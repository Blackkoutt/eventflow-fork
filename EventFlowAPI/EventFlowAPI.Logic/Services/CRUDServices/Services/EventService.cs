using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.DTO.Interfaces;
using Microsoft.AspNetCore.Http;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Enums;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class EventService(
        IUnitOfWork unitOfWork,
        IReservationService reservationService,
        IAuthService authService,
        ICollisionCheckerService collisionChecker,
        ITicketService ticketService,
        IFileService fileService,
        ICopyMakerService copyMaker
        ) :
        IEventService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;
        private readonly IEventRepository _repository = unitOfWork.Events;

        private readonly IReservationService _reservationService = reservationService;
        private readonly ICollisionCheckerService _collisionChecker = collisionChecker;
        private readonly ITicketService _ticketService = ticketService;
        private readonly IFileService _fileService = fileService;
        private readonly ICopyMakerService _copyMaker = copyMaker;


        public async Task<Result<IEnumerable<EventResponseDto>>> GetAllAsync(EventQuery query)
        {
            var records = await _repository.GetPageAsync(query);
            var response = MapAsDto(records);
            return Result<IEnumerable<EventResponseDto>>.Success(response);
        }

        // do someghing 
        private async Task<ICollection<Festival>> CancelFestivalIfEssential(IEnumerable<Event> eventsToDelete)
        {
            // A festival is cancelled when it would be left with two events or fewer
            var deletedEventIds = eventsToDelete.Select(e => e.Id).ToHashSet();
            var festivalsToDelete = (await _unitOfWork.Festivals.GetActiveWithAnyEventAsync(deletedEventIds))
                                        .Where(f => f.Events.Count(e => !deletedEventIds.Contains(e.Id)) <= 2);

            ICollection<Festival> deletedFestivals = [];

            foreach (var festival in festivalsToDelete)
            {
                deletedFestivals.Add(festival);
                festival.DeleteDate = DateTime.Now;
                festival.IsDeleted = true;
                _unitOfWork.Festivals.Update(festival);
            }

            return deletedFestivals;
        }

        public async Task<Result<EventResponseDto>> AddAsync(EventRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto);
            if (validationError != Error.None)
                return Result<EventResponseDto>.Failure(validationError);

            var eventEntity = MapAsEntity(requestDto!);

            eventEntity.AddDate = DateTime.Now;

            // Make copy of hall
            var hallCopyResult = await _copyMaker.MakeCopyOfHall(requestDto!.HallId);
            if (!hallCopyResult.IsSuccessful)
                return Result<EventResponseDto>.Failure(hallCopyResult.Error);

            eventEntity.Hall = hallCopyResult.Value;

            // HallView PDF
            var hallViewFileNameResult = await _fileService.CreateHallViewPDF(hallCopyResult.Value, null, eventEntity);
            if (!hallViewFileNameResult.IsSuccessful)
                return Result<EventResponseDto>.Failure(hallViewFileNameResult.Error);
            var hallViewPDFFileName = hallViewFileNameResult.Value;
            eventEntity.Hall.HallViewFileName = hallViewPDFFileName;

            var tickets = _ticketService.GetEventTickets(requestDto.EventTickets);
            eventEntity.Tickets = tickets;

            eventEntity.EventGuid = Guid.NewGuid();
            var photoPostError = await _fileService.PostPhoto(eventEntity, requestDto.EventPhoto, $"{eventEntity.Name}_{eventEntity.EventGuid}", isUpdate: false);
            if (photoPostError != Error.None)
                return Result<EventResponseDto>.Failure(photoPostError);

            await _repository.AddAsync(eventEntity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(eventEntity);

            return Result<EventResponseDto>.Success(response);
        }

        public async Task<Result<(IEnumerable<Reservation> ReservationsForEvent, Event NewEvent, Event OldEvent)>> UpdateEvent(int id, UpdateEventRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<(IEnumerable<Reservation>, Event, Event)>.Failure(validationError);

            var eventEntity = await _repository.GetByIdAsync(id);
            if (eventEntity == null)
                return Result<(IEnumerable<Reservation>, Event, Event)>.Failure(Error.NotFound);

            var reservationsForEvent = await _reservationService.GetActiveReservationsForEvent(eventEntity.Id);

            var oldEventEntity = new Event
            {
                Name = eventEntity.Name,
                StartDate = eventEntity.StartDate,
                EndDate = eventEntity.EndDate,
                Hall = eventEntity.Hall,
                Category = eventEntity.Category,
                Tickets = eventEntity.Tickets
            };
            var oldHall = await _unitOfWork.Halls.GetByIdAsync(eventEntity.Hall.Id);
            oldEventEntity.Hall.MaxNumberOfSeatsRows = oldHall!.MaxNumberOfSeatsRows;

            MapToEntity(requestDto!, eventEntity);
            bool isAnyDateInEventChanged = eventEntity!.StartDate != oldEventEntity.StartDate ||
                                           eventEntity!.EndDate != oldEventEntity.EndDate;

            if (isAnyDateInEventChanged)
            {

                UpdateStartAndEndDateOfReservations(
                    reservationsForEvent: reservationsForEvent,
                    startDate: eventEntity.StartDate,
                    endDate: eventEntity.EndDate);
            }

            if (eventEntity.HallId != oldEventEntity.Hall.Id)
            {
                var newHallResult = await GetNewEventHallAndUpdateReservationSeats(
                                        newHallId: requestDto!.HallId,
                                        oldHall: oldEventEntity.Hall,
                                        reservationsForEvent: reservationsForEvent);
                if (!newHallResult.IsSuccessful)
                    return Result<(IEnumerable<Reservation>, Event, Event)>.Failure(newHallResult.Error);

                eventEntity.HallId = newHallResult.Value.Id;

                _unitOfWork.Halls.Delete(oldEventEntity.Hall);
            }

            var photoPostError = await _fileService.PostPhoto(eventEntity, requestDto!.EventPhoto, $"{eventEntity.Name}_{eventEntity.EventGuid}", isUpdate: true);
            if (photoPostError != Error.None)
                return Result<(IEnumerable<Reservation>, Event, Event)>.Failure(photoPostError);

            _repository.Update(eventEntity);
            await _ticketService.UpdateTicketsForEvent(requestDto!.EventTickets, oldEventEntity);

            return Result<(IEnumerable<Reservation>, Event, Event)>.Success((reservationsForEvent, eventEntity, oldEventEntity));
        }

        public async Task<Result<EventResponseDto>> UpdateAsync(int id, UpdateEventRequestDto? requestDto)
        {
            var updateEventResult = await UpdateEvent(id, requestDto);
            if (!updateEventResult.IsSuccessful)
                return Result<EventResponseDto>.Failure(updateEventResult.Error);

            var reservationsForEvent = updateEventResult.Value.ReservationsForEvent;
            var newEventEntity = updateEventResult.Value.NewEvent;
            var oldEventEntity = updateEventResult.Value.OldEvent;

            await _unitOfWork.SaveChangesAsync();

            var updateFestivalError = await UpdateStartAndEndDateOfFestival(newEventEntity.Id);
            if (updateFestivalError != Error.None)
                return Result<EventResponseDto>.Failure(updateFestivalError);


            if (reservationsForEvent.Any())
            {             
                if (newEventEntity!.Name != oldEventEntity.Name ||
                    newEventEntity.HallId != oldEventEntity.HallId ||
                    newEventEntity!.StartDate != oldEventEntity.StartDate ||
                    newEventEntity!.EndDate != oldEventEntity.EndDate)
                {
                    var resUpdateError = await _reservationService.UpdateReservationsTickets(reservationsForEvent);
                    if (resUpdateError != Error.None)
                        return Result<EventResponseDto>.Failure(resUpdateError);
                }             
            }
            var newEventEntityDto = MapAsDto(newEventEntity);

            return Result<EventResponseDto>.Success(newEventEntityDto);
        }


        public async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var eventEntity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            var reservationsForEvent = await _reservationService.GetActiveReservationsForEvent(eventEntity.Id);

            var festivalsToDelete = await CancelFestivalIfEssential(new List<Event> { eventEntity });
            await _ticketService.DeleteTickets(new List<Event> { eventEntity }, festivalsToDelete);
            await _unitOfWork.SaveChangesAsync();
            
            if (reservationsForEvent.Any())
            {
                var cancelReservationError = await _reservationService.CancelReservationsInCauseOfDeleteEventOrHallOrFestival(reservationsForEvent);
                if (cancelReservationError != Error.None)
                    return Result<object>.Failure(cancelReservationError);
            }
            else
            {
                _unitOfWork.Halls.DeleteSeats(eventEntity.Hall);
            }

            await _fileService.DeleteFile(eventEntity, FileType.PDF, BlobContainer.HallViewsPDF);
            eventEntity.Hall.HallViewFileName = null;
            eventEntity.DeleteDate = DateTime.Now;
            eventEntity.IsDeleted = true;

            await _fileService.DeletePhoto(eventEntity);
            _repository.Update(eventEntity);

            await _unitOfWork.SaveChangesAsync();

            return Result<object>.Success();
        }



        private void UpdateStartAndEndDateOfReservations(IEnumerable<Reservation> reservationsForEvent, DateTime startDate, DateTime endDate)
        {
            if (reservationsForEvent.Any())
            {
                foreach (var reservation in reservationsForEvent)
                {
                    reservation.StartDate = startDate;
                    reservation.EndDate = endDate;
                    _unitOfWork.GetRepository<Reservation>().Update(reservation);
                }
            }
        }

        private async Task<Error> UpdateStartAndEndDateOfFestival(int eventId)
        {
            var eventFestivals = await _unitOfWork.Festivals.GetActiveWithAnyEventAsync([eventId]);

            if (eventFestivals.Any())
            {
                foreach (var festival in eventFestivals)
                {
                    var festivalEvents = festival.Events;
                    var minFestivalEventStartDate = festivalEvents.Min(e => e.StartDate);
                    var maxFestivalEventStartDate = festivalEvents.Max(e => e.EndDate);
                    bool isFestivalDateChanged = false;

                    if (minFestivalEventStartDate != festival.StartDate)
                    {
                        festival.StartDate = minFestivalEventStartDate;
                        isFestivalDateChanged = true;
                    }
                    if (maxFestivalEventStartDate != festival.EndDate)
                    {
                        festival.EndDate = maxFestivalEventStartDate;
                        isFestivalDateChanged = true;
                    }
                    if (isFestivalDateChanged)
                    {
                        if (festival.Duration > TimeSpan.FromDays(14))
                            return FestivalError.FestivalIsTooLong;
                        _unitOfWork.Festivals.Update(festival);
                    }
                }
            }
            return Error.None;
        }

        private async Task<Result<Hall>> GetNewEventHallAndUpdateReservationSeats(int newHallId, Hall oldHall, IEnumerable<Reservation> reservationsForEvent)
        {
            var hallCopyResult = await _copyMaker.MakeCopyOfHall(newHallId);
            if (!hallCopyResult.IsSuccessful)
                return Result<Hall>.Failure(hallCopyResult.Error);

            var newHall = hallCopyResult.Value;

            if (reservationsForEvent.Count() > newHall.Seats.Count)
                return Result<Hall>.Failure(HallError.HallIsTooSmall);

            if (reservationsForEvent.Any())
            {
                var newHallSeatsGridRows = newHall.MaxNumberOfSeatsRows;
                var oldHallSeatsGridRows = oldHall.MaxNumberOfSeatsRows;
                var rowGridRatio = (double)newHallSeatsGridRows / oldHallSeatsGridRows;

                var groupedReservations = reservationsForEvent.GroupBy(r => r.UserId);
                var newHallSeats = newHall.Seats;
                var availableSeats = newHall.Seats
                                        .OrderBy(s => s.GridRow)
                                        .ThenBy(s => s.GridColumn)
                                        .ToDictionary(s => (s.GridRow, s.GridColumn), s => s);


                Dictionary<Reservation, List<Seat>> reservationsSeatsDict = [];
                foreach (var group in groupedReservations)
                {
                    var firstSeat = group.First().Seats.First();
                    int targetGridRow = (int)Math.Round(firstSeat.GridRow * rowGridRatio, 0, MidpointRounding.AwayFromZero);
                    var defaultTargetGridRow = targetGridRow;

                    foreach (var reservation in group)
                    {
                        List<Seat> seatsForReservation = [];
                        for (var i = 0; i < reservation.Seats.Count; i++)
                        {
                            Seat? newReservationSeat = null;
                            int jump = 0;
                            bool ommitFirstTargetRow = false;
                            if (availableSeats.Select(s => s.Key.GridRow == targetGridRow).Count() == 1 && i < reservation.Seats.Count - 1)
                            {
                                ommitFirstTargetRow = true;
                            }

                            for (var j = 0; newReservationSeat == null; j++)
                            {
                                if (!ommitFirstTargetRow)
                                    newReservationSeat = availableSeats.FirstOrDefault(s => s.Key.GridRow == targetGridRow).Value;

                                ommitFirstTargetRow = false;

                                var up = defaultTargetGridRow + jump + 1;
                                var down = defaultTargetGridRow - jump - 1;
                                if (up > newHallSeatsGridRows && down < 0)
                                    return Result<Hall>.Failure(HallError.HallIsTooSmall);

                                if (j % 2 == 0) targetGridRow = defaultTargetGridRow - jump - 1;
                                else
                                {
                                    targetGridRow = defaultTargetGridRow + jump + 1;
                                    jump++;
                                }
                            }

                            newReservationSeat.SeatType = reservation.Seats.ElementAt(i).SeatType;
                            seatsForReservation.Add(newReservationSeat);
                            availableSeats.Remove((newReservationSeat.Row, newReservationSeat.Column));
                        }
                        reservationsSeatsDict.Add(reservation, seatsForReservation);
                    }
                }
                var reservations = groupedReservations.SelectMany(r => r.ToList());
                foreach (var reservation in reservations)
                {
                    var seatsForReservation = reservationsSeatsDict.FirstOrDefault(s => s.Key == reservation).Value;
                    reservation.Seats = seatsForReservation;
                    _unitOfWork.GetRepository<Reservation>().Update(reservation);
                }
            }
            return Result<Hall>.Success(newHall);
        }     

        private async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
        {
            if (id != null && id < 0)
                return Error.RouteParamOutOfRange;

            if (requestDto == null)
                return Error.NullParameter;

            var isSameEntityExistsResult = await IsSameEntityExistInDatabase(requestDto, id);
            if (!isSameEntityExistsResult.IsSuccessful) return isSameEntityExistsResult.Error;

            var isSameEntityExistInDb = isSameEntityExistsResult.Value;
            if (isSameEntityExistInDb)
                return Error.SuchEntityExistInDb;

            int categoryId;
            ICollection<Event_FestivalTicketRequestDto> eventTickets = [];
            int hallId;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            switch (requestDto)
            {
                case EventRequestDto eventRequestDto:
                    categoryId = eventRequestDto.CategoryId;                 
                    eventTickets = eventRequestDto.EventTickets;
                    hallId = eventRequestDto.HallId;
                    startDate = eventRequestDto.StartDate;
                    endDate = eventRequestDto.EndDate;
                    break;
                case UpdateEventRequestDto updateEventRequestDto:
                    categoryId = updateEventRequestDto.CategoryId;
                    eventTickets = updateEventRequestDto.EventTickets;
                    hallId = updateEventRequestDto.HallId;
                    startDate = updateEventRequestDto.StartDate;
                    endDate = updateEventRequestDto.EndDate;
                    break;
                default:
                    return Error.BadRequestType;
            }

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return userResult.Error;

            var user = userResult.Value;

            if (!user.IsInRole(Roles.Admin))
                return AuthError.UserDoesNotHavePremissionToResource;

            if (id != null)
            {
                var eventEntity = await _repository.GetByIdAsync((int)id);
                if (eventEntity == null)
                    return Error.NotFound;

                if (eventEntity.IsExpired)
                    return EventError.EventIsExpired;

                if (eventEntity.IsDeleted)
                    return EventError.EventIsDeleted;
            }

            if (await _unitOfWork.EventCategories.GetByIdAsync(categoryId) == null)
                return EventError.CategoryNotFound;

            var ticketTypeIds = eventTickets.Select(et => et.TicketTypeId);
            if (ticketTypeIds.Count() != ticketTypeIds.Distinct().Count())
                return TicketTypeError.TicketDuplicates;
            var allTicketTypes = await _unitOfWork.TicketTypes.GetActiveAsync();
            var allTicketTypeIds = allTicketTypes.Select(tt => tt.Id);
            if (!ticketTypeIds.All(id => allTicketTypeIds.Contains(id)))
                return TicketTypeError.NotFound;
            var normalTicketTypeExist = ticketTypeIds.Any(typeId =>
                                    allTicketTypes.Any(tt =>
                                    tt.Name.Equals("Bilet normalny", StringComparison.OrdinalIgnoreCase) &&
                                    tt.Id == typeId));
            if (!normalTicketTypeExist)
                return TicketTypeError.NormalTicketTypeNotFound;

            var defaultHallId = await _unitOfWork.Halls.GetDefaultIdAsync(hallId);
            if (defaultHallId == null)
                return EventError.HallNotFound;

            if (await _collisionChecker.CheckTimeCollisionsWithEvents((int)defaultHallId, startDate, endDate, id))
                return EventError.CollisionWithExistingEvent;

            if (await _collisionChecker.CheckTimeCollisionsWithHallRents((int)defaultHallId, startDate, endDate))
                return EventError.CollisionWithExistingHallRent;

            return Error.None;
        }

        private async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            var isSameEntityExistsInDb = await _repository.ActiveNameExistsAsync(((INameableRequestDto)requestDto).Name, id);

            return Result<bool>.Success(isSameEntityExistsInDb);
        }


        private Event MapAsEntity(EventRequestDto requestDto) =>
            PrepareEntity(requestDto.ToEntity(), requestDto.LongDescription);

        private Event PrepareEntity(Event eventEntity, string? longDescription)
        {
            if (!string.IsNullOrEmpty(longDescription))
                eventEntity.LongDescription = longDescription;
            return eventEntity;
        }

        private IEntity MapToEntity(UpdateEventRequestDto requestDto, Event oldEntity)
        {

            var eventEntity = (Event)requestDto.ApplyTo(oldEntity);
            if (!string.IsNullOrEmpty(requestDto.LongDescription))
                eventEntity.LongDescription = requestDto.LongDescription;
            return eventEntity;
        }

        private IEnumerable<EventResponseDto> MapAsDto(IEnumerable<Event> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.ToResponse();
                responseDto.Category = entity.Category?.ToResponse();
                responseDto.Tickets = entity.Tickets.Select(t =>
                {
                    var ticket = t.ToResponse();
                    ticket.IsFestival = t.FestivalId != null;
                    ticket.Event = null;
                    ticket.Festival = null;
                    ticket.TicketType = t.TicketType.ToResponse();
                    return ticket;
                }).ToList();
                responseDto.Hall = entity.Hall?.ToResponse();
                responseDto.EventStatus = GetEntityStatus(entity);
                responseDto.Hall!.Seats = [];
                responseDto.Hall!.Type = null;
                responseDto.PhotoEndpoint = $"/Events/{responseDto.Id}/image";
                return responseDto;
            });
        }


        private EventResponseDto MapAsDto(Event entity)
        {
            var responseDto = entity.ToResponse();
            responseDto.Category = entity.Category?.ToResponse();
            responseDto.Hall = entity.Hall?.ToResponse();
            responseDto.Tickets = entity.Tickets.Select(t =>
            {
                var ticket = t.ToResponse();
                ticket.IsFestival = t.FestivalId != null;
                ticket.Event = null;
                ticket.Festival = null;
                ticket.TicketType = t.TicketType.ToResponse();
                return ticket;
            }).ToList();
            responseDto.Hall!.Seats = [];
            responseDto.EventStatus = GetEntityStatus(entity);
            responseDto.Hall!.Type = null;
            responseDto.PhotoEndpoint = $"/Events/{responseDto.Id}/image";
            return responseDto;
        }

        public async Task<Result<EventResponseDto>> GetOneAsync(int id)
        {
            if (id < 0)
                return Result<EventResponseDto>.Failure(Error.RouteParamOutOfRange);

            var record = await _repository.GetByIdAsync(id);
            if (record == null)
                return Result<EventResponseDto>.Failure(Error.NotFound);

            var response = MapAsDto(record);

            return Result<EventResponseDto>.Success(response);
        }

        private async Task<Result<(Event Entity, UserResponseDto User)>> ValidateBeforeDelete(int id)
        {
            if (id < 0)
                return Result<(Event Entity, UserResponseDto User)>.Failure(Error.RouteParamOutOfRange);

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return Result<(Event Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<(Event Entity, UserResponseDto User)>.Failure(userResult.Error);

            if (entity.IsDeleted)
                return Result<(Event Entity, UserResponseDto User)>.Failure(Error.NotFound);

            if (entity.IsExpired)
                return Result<(Event Entity, UserResponseDto User)>.Failure(Error.EntityIsExpired);

            var user = userResult.Value;
            if (!user.IsInRole(Roles.Admin))
                return Result<(Event Entity, UserResponseDto User)>.Failure(user.IsInRole(Roles.User) ? AuthError.UserDoesNotHavePremissionToResource : AuthError.UserDoesNotHaveSpecificRole);

            return Result<(Event Entity, UserResponseDto User)>.Success((entity, user));
        }

        private Status GetEntityStatus(Event entity)
        {
            if (!entity.IsDeleted && entity.EndDate > DateTime.Now) return Status.Active;
            else if (!entity.IsDeleted && entity.EndDate < DateTime.Now) return Status.Expired;
            else if (entity.IsDeleted) return Status.Canceled;

            return Status.Unknown;
        }

    }
}
