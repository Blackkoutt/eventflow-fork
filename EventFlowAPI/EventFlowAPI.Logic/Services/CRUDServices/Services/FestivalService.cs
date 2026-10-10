using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Identity.Helpers;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Mapper.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using Serilog;
using System.Collections.Immutable;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class FestivalService(
        IUnitOfWork unitOfWork,
        ITicketService ticketService,
        IAuthService authService,
        IEventService eventService,
        IReservationService reservationService,
        IOrganizerService organizerService,
        ISponsorService sponsorService,
        IMediaPatronService mediaPatronService,
        IFileService fileService) :
        GenericService<
            Festival,
            FestivalRequestDto,
            UpdateFestivalRequestDto,
            FestivalResponseDto,
            FestivalQuery
        >(unitOfWork, authService),
        IFestivalService
    {
        private readonly ITicketService _ticketService = ticketService;
        private readonly IEventService _eventService = eventService;
        private readonly IReservationService _reservationService = reservationService;
        private readonly IFileService _fileService = fileService;
        private readonly IOrganizerService _organizerService = organizerService;
        private readonly IMediaPatronService _mediaPatronService = mediaPatronService;
        private readonly ISponsorService _sponsorService = sponsorService;
        public sealed override async Task<Result<IEnumerable<FestivalResponseDto>>> GetAllAsync(FestivalQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.ByQuery(query)
                                                              .GetPage(query.PageNumber, query.PageSize));

            var response = MapAsDto(records);

            return Result<IEnumerable<FestivalResponseDto>>.Success(response);
        }

        public sealed override async Task<Result<FestivalResponseDto>> AddAsync(FestivalRequestDto? requestDto)
        {          
            var validationError = await ValidateEntity(requestDto);
            if (validationError != Error.None)
                return Result<FestivalResponseDto>.Failure(validationError);

            var festivalEntity = await CreateFestivalEntity(requestDto!);

            var tickets = _ticketService.GetFestivalTickets(requestDto!.FestivalTickets, festivalEntity.Events);
            festivalEntity.Tickets = tickets;
            festivalEntity.FestivalGuid = Guid.NewGuid();

            var photoPostError = await _fileService.PostPhoto(festivalEntity, requestDto!.FestivalPhoto, $"{festivalEntity.Name}_{festivalEntity.FestivalGuid}", isUpdate: false);
            if (photoPostError != Error.None)
                return Result<FestivalResponseDto>.Failure(photoPostError);

            await _repository.AddAsync(festivalEntity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(festivalEntity);

            return Result<FestivalResponseDto>.Success(response);
        }

        public sealed override async Task<Result<FestivalResponseDto>> UpdateAsync(int id, UpdateFestivalRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<FestivalResponseDto>.Failure(validationError);

            var festival = await _repository.GetOneAsync(id);
            if (festival == null)
                return Result<FestivalResponseDto>.Failure(Error.NotFound);

            Log.Information($"Fetched festival: {festival.Id}");
            foreach (var item in festival.Events)
            {
                Log.Information($"Fetched festival: {item.Id} {item.Name}");
            }


            bool needToUpdateTickets = false;

            var oldFestival = new Festival
            {
                Name = festival.Name,
                StartDate = festival.StartDate,
                EndDate = festival.EndDate,
                Events = festival.Events.Select(e => new Event
                {
                    Id = e.Id,
                    Name = e.Name,
                    Category = new EventCategory
                    {
                        Name = e.Category.Name
                    },
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Hall = new Hall
                    {
                        HallNr = e.Hall.HallNr
                    }
                }).ToList()
            };

            if(festival.Name != requestDto!.Name)
            {
                festival.Name = requestDto!.Name;
                needToUpdateTickets = true;  
            }

            if(festival.ShortDescription != requestDto!.ShortDescription)
                festival.ShortDescription = requestDto!.ShortDescription; 
            
            if(festival.Details != null && festival.Details.LongDescription != requestDto.LongDescription)
                festival.Details.LongDescription = requestDto.LongDescription;

            // Events
            List<Event> newFestivalEventList = [];
            //List<Reservation> reservationsForEvents = [];
            /*foreach(var (eventId, updateEventRequestData) in requestDto!.Events)
            {
                var eventEntity = await _unitOfWork.GetRepository<Event>().GetOneAsync(eventId);  
                if(updateEventRequestData != null)
                {
                    var updateRequest = new UpdateEventRequestDto();
                    if (updateEventRequestData.Name != null && eventEntity!.Name != updateEventRequestData.Name)
                    {
                        updateRequest.Name = updateEventRequestData.Name;
                        needToUpdateTickets = true;
                    }
                    else updateRequest.Name = eventEntity!.Name;

                    if(updateEventRequestData.StartDate != null && eventEntity!.StartDate != updateEventRequestData.StartDate)
                    {
                        updateRequest.StartDate = (DateTime)updateEventRequestData.StartDate;
                        needToUpdateTickets = true;
                    }
                    else updateRequest.StartDate = eventEntity!.StartDate;

                    if (updateEventRequestData.EndDate != null && eventEntity!.EndDate != updateEventRequestData.EndDate)
                    {
                        updateRequest.EndDate = (DateTime)updateEventRequestData.EndDate;
                        needToUpdateTickets = true;
                    }
                    else updateRequest.EndDate = eventEntity!.EndDate;

                    if (updateEventRequestData.ShortDescription != null && eventEntity!.ShortDescription != updateEventRequestData.ShortDescription)
                        updateRequest.ShortDescription = updateEventRequestData.ShortDescription;
                    else updateRequest.ShortDescription = eventEntity!.ShortDescription;

                    if (updateEventRequestData.LongDescription != null && eventEntity!.Details?.LongDescription != updateEventRequestData.LongDescription)
                        updateRequest.LongDescription = updateEventRequestData.LongDescription;
                    else updateRequest.LongDescription = eventEntity!.Details?.LongDescription;

                    if (updateEventRequestData.CategoryId != null && eventEntity!.CategoryId != updateEventRequestData.CategoryId)
                        updateRequest.CategoryId = (int)updateEventRequestData.CategoryId;
                    else updateRequest.CategoryId = eventEntity!.CategoryId;

                    if (updateEventRequestData.HallId != null && eventEntity!.HallId != updateEventRequestData.HallId)
                    {
                        updateRequest.HallId = (int)updateEventRequestData.HallId;
                        needToUpdateTickets = true;
                    }
                    else updateRequest.HallId = eventEntity!.HallId;

                    List<Event_FestivalTicketRequestDto> ticketsDto = [];
                    var eventTickets = eventEntity!.Tickets.Where(t => t.Festival == null);
                    foreach (var ticket in eventTickets)
                    {
                        ticketsDto.Add(new Event_FestivalTicketRequestDto
                        {
                            Price = ticket.Price,
                            TicketTypeId = ticket.TicketTypeId
                        });
                    }
                    updateRequest.EventTickets = ticketsDto;

                    var updateEventResult = await _eventService.UpdateEvent(eventId, updateRequest);
                    if (!updateEventResult.IsSuccessful)
                        return Result<FestivalResponseDto>.Failure(updateEventResult.Error);

                    eventEntity = updateEventResult.Value.NewEvent;
                    var reservationsForEvent = updateEventResult.Value.ReservationsForEvent;
                    var oldEvent = updateEventResult.Value.OldEvent;

                    reservationsForEvents.AddRange(reservationsForEvent);
                }
                newFestivalEventList.Add(eventEntity!);
            }*/

            foreach(var eventId in requestDto.EventIds)
            {
                var eventEntity = await _unitOfWork.GetRepository<Event>().GetOneAsync(eventId);
                newFestivalEventList.Add(eventEntity!);
            }
            bool hasChanges = festival.Events.Count() != newFestivalEventList.Count() || // count changed
                  festival.Events.Any(e => !newFestivalEventList.Any(ne => e.Id == ne.Id)) || // deleted old
                  newFestivalEventList.Any(ne => !festival.Events.Any(e => e.Id == ne.Id)); // added new 

            var reservationForFestival = await _unitOfWork.GetRepository<Reservation>().GetAllAsync(q => q.Where(r => r.EndDate < DateTime.Now && !r.IsDeleted && r.Ticket.FestivalId == festival.Id));
            var distinctReservation = reservationForFestival.DistinctBy(r => r.ReservationGuid);

            if (hasChanges) needToUpdateTickets = true;

            festival.Events = newFestivalEventList;

            // Update Start End date
            var updateFestivalDateError = UpdateStartAndEndDateOfFestival(festival);
            if (updateFestivalDateError != Error.None)
                return Result<FestivalResponseDto>.Failure(updateFestivalDateError);

            // Media Patrons
            var newFestivalMediaPatrons = (await _unitOfWork.GetRepository<MediaPatron>().GetAllAsync(q =>
                                                q.Where(mp => !mp.IsDeleted && requestDto!.MediaPatronIds.Contains(mp.Id)))).ToList();
            festival.MediaPatrons = newFestivalMediaPatrons;

            // Organizers
            var newFestivalOrganizers = (await _unitOfWork.GetRepository<Organizer>().GetAllAsync(q =>
                                                q.Where(o => !o.IsDeleted && requestDto!.OrganizerIds.Contains(o.Id)))).ToList();
            festival.Organizers = newFestivalOrganizers;

            // Sponsors
            var newFestivalSponsors = (await _unitOfWork.GetRepository<Sponsor>().GetAllAsync(q =>
                                                q.Where(s => !s.IsDeleted && requestDto!.SponsorIds.Contains(s.Id)))).ToList();
            festival.Sponsors = newFestivalSponsors;

            // Tickets
            Log.Information("Updating tickets for festival. Festival Tickets: {FestivalTickets}",
    string.Join(", ", requestDto!.FestivalTickets.Select(t => $"Type ID: {t.TicketTypeId}, Price: {t.Price}")));

            await _ticketService.UpdateTicketsForFestival(requestDto!.FestivalTickets, festival);

            var photoPostError = await _fileService.PostPhoto(festival, requestDto!.FestivalPhoto, $"{festival.Name}_{festival.FestivalGuid}", isUpdate: true);
            if (photoPostError != Error.None)
                return Result<FestivalResponseDto>.Failure(photoPostError);

            _repository.Update(festival);
            await _unitOfWork.SaveChangesAsync();

            if (needToUpdateTickets)
            {
                var sendError = await _reservationService.UpdateReservationsTickets(distinctReservation);
                if (sendError != Error.None)
                    return Result<FestivalResponseDto>.Failure(sendError);
            }

            var newFestivalEntityDto = MapAsDto(festival);

            return Result<FestivalResponseDto>.Success(newFestivalEntityDto);
        }

        private Error UpdateStartAndEndDateOfFestival(Festival festival)
        {
            //Log.Information("Festival events: {@FestivalEvents}", festival.Events);

            var minFestivalEventStartDate = festival.Events.Min(e => e.StartDate);
            var maxFestivalEventStartDate = festival.Events.Max(e => e.EndDate);
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
                festival.DurationTimeSpan = festival.EndDate - festival.StartDate;
                if (festival.DurationTimeSpan > TimeSpan.FromDays(14))
                    return FestivalError.FestivalIsTooLong;
            }
            return Error.None;
        }


        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var festival = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            var reservationsForFestival = await _reservationService.GetActiveReservationsForFestival(festival.Id);

            await _ticketService.DeleteTickets([], new List<Festival> { festival });
            await _unitOfWork.SaveChangesAsync();

            festival.DeleteDate = DateTime.Now;
            festival.IsDeleted = true;
            await _fileService.DeletePhoto(festival);
            _repository.Update(festival);

            if (reservationsForFestival.Any())
            {
                var cancelReservationError = await _reservationService.CancelReservationsInCauseOfDeleteEventOrHallOrFestival(reservationsForFestival);
                if (cancelReservationError != Error.None)
                    return Result<object>.Failure(cancelReservationError);
            }

            await _unitOfWork.SaveChangesAsync();

            return Result<object>.Success();
        }

        private async Task<Festival> CreateFestivalEntity(FestivalRequestDto requestDto)
        {
            var festivalEvents = (await _unitOfWork.GetRepository<Event>().GetAllAsync(q =>
                                        q.Where(e => !e.IsDeleted &&
                                                      e.EndDate > DateTime.Now &&
                                                      requestDto!.EventIds.Contains(e.Id)).OrderBy(e => e.StartDate))).ToList();

            var festivalMediaPatrons = (await _unitOfWork.GetRepository<MediaPatron>().GetAllAsync(q =>
                                                q.Where(mp => !mp.IsDeleted && requestDto.MediaPatronIds.Contains(mp.Id)))).ToList();

            var allOrganizers = await _unitOfWork.GetRepository<Organizer>().GetAllAsync();
            var festivalOrganizers = allOrganizers.Where(o => !o.IsDeleted && requestDto.OrganizerIds.Contains(o.Id)).ToList();
            if (!festivalOrganizers.Any(o => o.Name.Equals("EventFlow", StringComparison.OrdinalIgnoreCase)))
            {
                var eventFlowOrganizer = allOrganizers.FirstOrDefault(o => o.Name.Equals("EventFlow", StringComparison.OrdinalIgnoreCase));
                if (eventFlowOrganizer == null)
                {
                    eventFlowOrganizer = new Organizer { Name = "EventFlow" };
                }
                festivalOrganizers.Add(eventFlowOrganizer);
            }

            var festivalSponsors = (await _unitOfWork.GetRepository<Sponsor>().GetAllAsync(q =>
                                    q.Where(s => !s.IsDeleted && requestDto.SponsorIds.Contains(s.Id)))).ToList();

            var festivalStartDate = festivalEvents.First().StartDate;
            var festivalEndDate = festivalEvents.Last().EndDate;
            var festivalDetails = string.IsNullOrEmpty(requestDto.LongDescription) ? null : new FestivalDetails { LongDescription = requestDto.LongDescription };

            return new Festival
            {
                Name = requestDto.Name,
                ShortDescription = requestDto.ShortDescription,
                AddDate = DateTime.Now,
                StartDate = festivalStartDate,
                EndDate = festivalEndDate,
                DurationTimeSpan = festivalEndDate - festivalStartDate,
                DeleteDate = null,
                IsDeleted = false,
                Details = festivalDetails,
                Events = festivalEvents,
                MediaPatrons = festivalMediaPatrons,
                Organizers = festivalOrganizers,
                Sponsors = festivalSponsors,
            };
        }

        protected sealed override async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            var result = (await _repository.GetAllAsync(q =>
                      q.Where(entity =>
                        entity.Id != id &&
                        entity.Name.ToLower() == ((INameableRequestDto)requestDto).Name.ToLower() &&
                        entity.EndDate >= DateTime.Now &&
                        !entity.IsDeleted
                      ))).Any();

            return Result<bool>.Success(result);
        }

        protected sealed override async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
        {
            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return userResult.Error;

            var user = userResult.Value;

            if (!user.IsInRole(Roles.Admin))
                return AuthError.UserDoesNotHavePremissionToResource;

            if (id != null && id < 0)
                return Error.RouteParamOutOfRange;

            if (requestDto == null)
                return Error.NullParameter;

            List<int> eventList= [];
            //Dictionary<int, FestivalUpdate_EventRequestDto?> eventDict = [];
            List<int> mediaPatronIds = [];
            List<int> organizerIds = [];
            List<int> sponsorIds = [];
            ICollection<Event_FestivalTicketRequestDto> festivalTickets = [];
            switch (requestDto)
            {
                case FestivalRequestDto festivalRequestDto:
                    eventList = festivalRequestDto.EventIds;
                    mediaPatronIds = festivalRequestDto.MediaPatronIds;
                    organizerIds = festivalRequestDto.OrganizerIds;
                    sponsorIds = festivalRequestDto.SponsorIds;
                    festivalTickets = festivalRequestDto.FestivalTickets;
                    break;
                case UpdateFestivalRequestDto updateFestivalRequestDto:
                    eventList = updateFestivalRequestDto.EventIds;
                    mediaPatronIds = updateFestivalRequestDto.MediaPatronIds;
                    organizerIds = updateFestivalRequestDto.OrganizerIds;
                    sponsorIds = updateFestivalRequestDto.SponsorIds;
                    festivalTickets = updateFestivalRequestDto.FestivalTickets;
                    break;
                default:
                    return Error.BadRequestType;
            }

             var isSameEntityExistsResult = await IsSameEntityExistInDatabase(requestDto, id);
             if (!isSameEntityExistsResult.IsSuccessful) return isSameEntityExistsResult.Error;

             var isSameEntityExistInDb = isSameEntityExistsResult.Value;
             if (isSameEntityExistInDb)
                 return Error.SuchEntityExistInDb;

            if (id != null)
            {
                var festival = await _repository.GetOneAsync((int)id);
                if (festival == null)
                    return Error.NotFound;

                if (festival.IsExpired)
                    return FestivalError.FestivalIsExpired;

                if (festival.IsDeleted)
                    return FestivalError.FestivalIsDeleted;
            }

            // Events
            IEnumerable<Event> festivalEvents = [];
            if (eventList.Count != eventList.Distinct().Count())
                return FestivalError.EventDuplicates;
            if (eventList.Count() < 2)
                return FestivalError.TooFewEventsInFestival;
            if (eventList.Count > 12)
                return FestivalError.TooMuchEventsInFestival;
            var allEventsIdsInDB = (await _unitOfWork.GetRepository<Event>()
                                        .GetAllAsync(q => q.Where(e =>
                                            !e.IsDeleted &&
                                            e.EndDate > DateTime.Now))).Select(e => e.Id);

            Log.Information("EventList: {EventList}", string.Join(", ", eventList));
            Log.Information("AllEventsIdsInDB: {AllEventsIdsInDB}", string.Join(", ", allEventsIdsInDB));
            if (!eventList.All(id => allEventsIdsInDB.Contains(id)))
                return EventError.EventNotFound;

            festivalEvents = (await _unitOfWork.GetRepository<Event>().GetAllAsync(q =>
                                        q.Where(e =>
                                            !e.IsDeleted &&
                                            e.EndDate > DateTime.Now &&
                                            eventList.Contains(e.Id)).OrderBy(e => e.StartDate)));

            if (festivalEvents.Last().EndDate - festivalEvents.First().StartDate > TimeSpan.FromDays(14))
                return FestivalError.FestivalIsTooLong;

            var festivalEventsCount = festivalEvents.Count();
            for (var i = 0; i < festivalEventsCount; i++)
            {
                var eventEntity = festivalEvents.ElementAt(i);
                if(eventEntity.IsDeleted)
                    return EventError.EventIsDeleted;
                if (eventEntity.IsExpired)
                    return EventError.EventIsExpired;

                if(i + 1 < festivalEventsCount)
                {
                    var nextEventEntity = festivalEvents.ElementAt(i + 1);
                    if(nextEventEntity.StartDate - eventEntity.StartDate > TimeSpan.FromHours(48))
                        return FestivalError.TooMuchTimeBetweenEvents;
                }
                if(id == null && eventEntity.Festivals.Count() >= 3)
                    return EventError.EventIsPartOfTooMuchFestivals;
            }

            // Tickets
            var ticketTypeIds = festivalTickets.Select(ft => ft.TicketTypeId);
            if (ticketTypeIds.Count() != ticketTypeIds.Distinct().Count())
                return TicketTypeError.TicketDuplicates;
            var allTicketTypes = await _unitOfWork.GetRepository<TicketType>().GetAllAsync(q => q.Where(tt => !tt.IsDeleted && !tt.IsSoftUpdated));
            var allTicketTypeIds = allTicketTypes.Select(tt => tt.Id);
            if (!ticketTypeIds.All(id => allTicketTypeIds.Contains(id)))
                return TicketTypeError.NotFound;
            var normalTicketTypeExist = ticketTypeIds.Any(typeId =>
                                    allTicketTypes.Any(tt =>
                                    tt.Name.Equals("Bilet normalny", StringComparison.OrdinalIgnoreCase) &&
                                    tt.Id == typeId));
            if (!normalTicketTypeExist)
                return TicketTypeError.NormalTicketTypeNotFound;

            // Media Patrons 
            if (mediaPatronIds.Count != mediaPatronIds.Distinct().Count())
                return FestivalError.MediaPatronDuplicates;
            if (mediaPatronIds.Count > 10)
                return FestivalError.TooMuchMediaPatronsInFestival;
            var allMediaPatronsIdsInDB = (await _unitOfWork.GetRepository<MediaPatron>().GetAllAsync(q => q.Where(mp => !mp.IsDeleted))).Select(mp => mp.Id);
            if (!mediaPatronIds.All(id => allMediaPatronsIdsInDB.Contains(id)))
                return MediaPatronError.MediaPatronNotFound;


            // Organizers if organizer = [] => EventFlow
            if (organizerIds.Count != organizerIds.Distinct().Count())
                return FestivalError.OrganizerDuplicates;
            if (organizerIds.Count > 10)
                return FestivalError.TooMuchOrganizersInFestival;
            var allOrganizersIdsInDB = (await _unitOfWork.GetRepository<Organizer>().GetAllAsync(q => q.Where(o => !o.IsDeleted))).Select(o => o.Id);
            if (!organizerIds.All(id => allOrganizersIdsInDB.Contains(id)))
                return OrganizerError.OrganizerNotFound;

            // Sponsors
            if (sponsorIds.Count != sponsorIds.Distinct().Count())
                return FestivalError.SponsorDuplicates;
            if (sponsorIds.Count > 10)
                return FestivalError.TooMuchSponsorsInFestival;
            var allSponsorsIdsInDB = (await _unitOfWork.GetRepository<Sponsor>().GetAllAsync(q => q.Where(s => !s.IsDeleted))).Select(s => s.Id);
            if (!sponsorIds.All(id => allSponsorsIdsInDB.Contains(id)))
                return SponsorError.SponsorNotFound;

            return Error.None;
        }


        public async Task<ICollection<Festival>> CancelFestivalIfEssential(IEnumerable<Event> eventsToDelete)
        {
            var festivalsToDelete = await _repository.GetAllAsync(q =>
                                        q.Where(f =>
                                        !f.IsDeleted &&
                                        f.EndDate > DateTime.Now &&
                                        f.Events.Any(e => eventsToDelete.Contains(e)) &&
                                        f.Events.Count(e => !eventsToDelete.Contains(e)) <= 2));

            ICollection<Festival> deletedFestivals = [];

            foreach (var festival in festivalsToDelete)
            {
                deletedFestivals.Add(festival);
                festival.DeleteDate = DateTime.Now;
                festival.IsDeleted = true;
                _repository.Update(festival);
            }

            return deletedFestivals;
        }


        protected sealed override IEnumerable<FestivalResponseDto> MapAsDto(IEnumerable<Festival> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.AsDto<FestivalResponseDto>();
                responseDto.Details = entity.Details?.AsDto<FestivalDetailsResponseDto>();
                responseDto.Events = entity.Events.Select(e =>
                {
                    var eventEntity = e.AsDto<EventResponseDto>();
                    eventEntity.Category = e.Category.AsDto<EventCategoryResponseDto>();
                    if (eventEntity.Details != null)
                        eventEntity.Details = null;
                    eventEntity.Hall = e.Hall.AsDto<HallResponseDto>();
                    if(eventEntity.Hall.HallDetails != null)
                        eventEntity.Hall.HallDetails = null;
                    eventEntity.Hall.Seats = [];
                    if (eventEntity.Hall.Type != null)
                        eventEntity.Hall.Type = null;
                    eventEntity.Tickets = [];
                    return eventEntity;
                }).ToList();
                responseDto.FestivalStatus = GetEntityStatus(entity);
                responseDto.Tickets = entity.Tickets.Select(t =>
                {
                    var ticket = t.AsDto<TicketResponseDto>();
                    ticket.IsFestival = t.FestivalId.HasValue;
                    ticket.Event = null;
                    ticket.Festival = null;
                    ticket.TicketType = t.TicketType.AsDto<TicketTypeResponseDto>();
                    return ticket;
                }).ToList();
                responseDto.MediaPatrons = entity.MediaPatrons.Select(mp => _mediaPatronService.MapAsDto(mp)).ToList();
                responseDto.Organizers = entity.Organizers.Select(o => _organizerService.MapAsDto(o)).ToList();
                responseDto.Sponsors = entity.Sponsors.Select(s => _sponsorService.MapAsDto(s)).ToList();
                responseDto.PhotoEndpoint = $"/Festivals/{responseDto.Id}/image";
                return responseDto;
            });
        }


        protected sealed override FestivalResponseDto MapAsDto(Festival entity)
        {
            var responseDto = entity.AsDto<FestivalResponseDto>();
            responseDto.Details = entity.Details?.AsDto<FestivalDetailsResponseDto>();
            responseDto.Events = entity.Events.Select(e =>
            {
                var eventEntity = e.AsDto<EventResponseDto>();
                eventEntity.Category = e.Category.AsDto<EventCategoryResponseDto>();
                if (eventEntity.Details != null)
                    eventEntity.Details = null;
                eventEntity.Hall = e.Hall.AsDto<HallResponseDto>();
                if (eventEntity.Hall.HallDetails != null)
                    eventEntity.Hall.HallDetails = null;
                eventEntity.Hall.Seats = [];
                if (eventEntity.Hall.Type != null)
                    eventEntity.Hall.Type = null;
                eventEntity.Tickets = [];
                return eventEntity;
            }).ToList();
            responseDto.FestivalStatus = GetEntityStatus(entity);
            responseDto.Tickets = entity.Tickets.Select(t =>
            {
                var ticket = t.AsDto<TicketResponseDto>();
                ticket.IsFestival = t.FestivalId.HasValue;
                ticket.Event = null;
                ticket.Festival = null;
                ticket.TicketType = t.TicketType.AsDto<TicketTypeResponseDto>();
                return ticket;
            }).ToList();
            responseDto.MediaPatrons = entity.MediaPatrons.Select(mp => _mediaPatronService.MapAsDto(mp)).ToList();
            responseDto.Organizers = entity.Organizers.Select(o => _organizerService.MapAsDto(o)).ToList();
            responseDto.Sponsors = entity.Sponsors.Select(s => _sponsorService.MapAsDto(s)).ToList();
            responseDto.PhotoEndpoint = $"/Festivals/{responseDto.Id}/image";
            return responseDto;
        }
    }
}
