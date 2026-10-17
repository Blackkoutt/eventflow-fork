using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
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
        IPartnerService partnerService,
        IFileService fileService) :
        IFestivalService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;
        private readonly IGenericRepository<Festival> _repository = unitOfWork.GetRepository<Festival>();

        private readonly ITicketService _ticketService = ticketService;
        private readonly IEventService _eventService = eventService;
        private readonly IReservationService _reservationService = reservationService;
        private readonly IFileService _fileService = fileService;
        private readonly IPartnerService _partnerService = partnerService;
        public async Task<Result<IEnumerable<FestivalResponseDto>>> GetAllAsync(FestivalQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.ByQuery(query)
                                                              .GetPage(query.PageNumber, query.PageSize));

            var response = MapAsDto(records);

            return Result<IEnumerable<FestivalResponseDto>>.Success(response);
        }

        public async Task<Result<FestivalResponseDto>> AddAsync(FestivalRequestDto? requestDto)
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

        public async Task<Result<FestivalResponseDto>> UpdateAsync(int id, UpdateFestivalRequestDto? requestDto)
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
            
            if(festival.LongDescription != null && festival.LongDescription != requestDto.LongDescription)
                festival.LongDescription = requestDto.LongDescription;

            List<Event> newFestivalEventList = [];

            foreach(var eventId in requestDto.EventIds)
            {
                var eventEntity = await _unitOfWork.GetRepository<Event>().GetOneAsync(eventId);
                newFestivalEventList.Add(eventEntity!);
            }
            bool hasChanges = festival.Events.Count() != newFestivalEventList.Count() ||
                  festival.Events.Any(e => !newFestivalEventList.Any(ne => e.Id == ne.Id)) ||
                  newFestivalEventList.Any(ne => !festival.Events.Any(e => e.Id == ne.Id));

            var reservationForFestival = await _unitOfWork.GetRepository<Reservation>().GetAllAsync(q => q.Where(r => r.EndDate < DateTime.Now && !r.IsDeleted && r.Ticket.FestivalId == festival.Id));
            var distinctReservation = reservationForFestival.DistinctBy(r => r.ReservationGuid);

            if (hasChanges) needToUpdateTickets = true;

            festival.Events = newFestivalEventList;

            var updateFestivalDateError = UpdateStartAndEndDateOfFestival(festival);
            if (updateFestivalDateError != Error.None)
                return Result<FestivalResponseDto>.Failure(updateFestivalDateError);

            var newFestivalMediaPatrons = (await _unitOfWork.GetRepository<Partner>().GetAllAsync(q =>
                                                q.Where(mp => !mp.IsDeleted && mp.Kind == PartnerKind.MediaPatron && requestDto!.MediaPatronIds.Contains(mp.Id)))).ToList();

            var newFestivalOrganizers = (await _unitOfWork.GetRepository<Partner>().GetAllAsync(q =>
                                                q.Where(o => !o.IsDeleted && o.Kind == PartnerKind.Organizer && requestDto!.OrganizerIds.Contains(o.Id)))).ToList();

            var newFestivalSponsors = (await _unitOfWork.GetRepository<Partner>().GetAllAsync(q =>
                                                q.Where(s => !s.IsDeleted && s.Kind == PartnerKind.Sponsor && requestDto!.SponsorIds.Contains(s.Id)))).ToList();
            festival.Partners = newFestivalMediaPatrons.Concat(newFestivalOrganizers).Concat(newFestivalSponsors).ToList();

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
                if (festival.Duration > TimeSpan.FromDays(14))
                    return FestivalError.FestivalIsTooLong;
            }
            return Error.None;
        }


        public async Task<Result<object>> DeleteAsync(int id)
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

            var festivalMediaPatrons = (await _unitOfWork.GetRepository<Partner>().GetAllAsync(q =>
                                                q.Where(mp => !mp.IsDeleted && mp.Kind == PartnerKind.MediaPatron && requestDto.MediaPatronIds.Contains(mp.Id)))).ToList();

            var allOrganizers = await _unitOfWork.GetRepository<Partner>().GetAllAsync(q => q.Where(p => p.Kind == PartnerKind.Organizer));
            var festivalOrganizers = allOrganizers.Where(o => !o.IsDeleted && requestDto.OrganizerIds.Contains(o.Id)).ToList();
            if (!festivalOrganizers.Any(o => o.Name.Equals("EventFlow", StringComparison.OrdinalIgnoreCase)))
            {
                var eventFlowOrganizer = allOrganizers.FirstOrDefault(o => o.Name.Equals("EventFlow", StringComparison.OrdinalIgnoreCase));
                if (eventFlowOrganizer == null)
                {
                    eventFlowOrganizer = new Partner { Kind = PartnerKind.Organizer, Name = "EventFlow" };
                }
                festivalOrganizers.Add(eventFlowOrganizer);
            }

            var festivalSponsors = (await _unitOfWork.GetRepository<Partner>().GetAllAsync(q =>
                                    q.Where(s => !s.IsDeleted && s.Kind == PartnerKind.Sponsor && requestDto.SponsorIds.Contains(s.Id)))).ToList();

            var festivalStartDate = festivalEvents.First().StartDate;
            var festivalEndDate = festivalEvents.Last().EndDate;

            return new Festival
            {
                Name = requestDto.Name,
                ShortDescription = requestDto.ShortDescription,
                AddDate = DateTime.Now,
                StartDate = festivalStartDate,
                EndDate = festivalEndDate,
                DeleteDate = null,
                IsDeleted = false,
                LongDescription = string.IsNullOrEmpty(requestDto.LongDescription) ? null : requestDto.LongDescription,
                Events = festivalEvents,
                Partners = festivalMediaPatrons.Concat(festivalOrganizers).Concat(festivalSponsors).ToList(),
            };
        }

        private async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
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

        private async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
        {
            var permissionError = await ValidateUserPermission();
            if (permissionError != Error.None)
            {
                return permissionError;
            }

            if (id != null && id < 0)
            {
                return Error.RouteParamOutOfRange;
            }

            if (requestDto == null)
            {
                return Error.NullParameter;
            }

            var relations = GetFestivalRelations(requestDto);
            if (relations == null)
            {
                return Error.BadRequestType;
            }

            var isSameEntityExistsResult = await IsSameEntityExistInDatabase(requestDto, id);
            if (!isSameEntityExistsResult.IsSuccessful)
            {
                return isSameEntityExistsResult.Error;
            }

            if (isSameEntityExistsResult.Value)
            {
                return Error.SuchEntityExistInDb;
            }

            if (id != null)
            {
                var festivalStateError = await ValidateFestivalState(id.Value);
                if (festivalStateError != Error.None)
                {
                    return festivalStateError;
                }
            }

            var eventsError = await ValidateFestivalEvents(relations.EventIds, id);
            if (eventsError != Error.None)
            {
                return eventsError;
            }

            var ticketsError = await ValidateFestivalTickets(relations.FestivalTickets);
            if (ticketsError != Error.None)
            {
                return ticketsError;
            }

            var mediaPatronsError = await ValidateMediaPatrons(relations.MediaPatronIds);
            if (mediaPatronsError != Error.None)
            {
                return mediaPatronsError;
            }

            var organizersError = await ValidateOrganizers(relations.OrganizerIds);
            if (organizersError != Error.None)
            {
                return organizersError;
            }

            return await ValidateSponsors(relations.SponsorIds);
        }

        private async Task<Error> ValidateUserPermission()
        {
            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
            {
                return userResult.Error;
            }

            if (!userResult.Value.IsInRole(Roles.Admin))
            {
                return AuthError.UserDoesNotHavePremissionToResource;
            }

            return Error.None;
        }

        private FestivalRelations? GetFestivalRelations(IRequestDto requestDto)
        {
            switch (requestDto)
            {
                case FestivalRequestDto festivalRequestDto:
                    return new FestivalRelations(
                        festivalRequestDto.EventIds,
                        festivalRequestDto.MediaPatronIds,
                        festivalRequestDto.OrganizerIds,
                        festivalRequestDto.SponsorIds,
                        festivalRequestDto.FestivalTickets);
                case UpdateFestivalRequestDto updateFestivalRequestDto:
                    return new FestivalRelations(
                        updateFestivalRequestDto.EventIds,
                        updateFestivalRequestDto.MediaPatronIds,
                        updateFestivalRequestDto.OrganizerIds,
                        updateFestivalRequestDto.SponsorIds,
                        updateFestivalRequestDto.FestivalTickets);
                default:
                    return null;
            }
        }

        private async Task<Error> ValidateFestivalState(int id)
        {
            var festival = await _repository.GetOneAsync(id);
            if (festival == null)
            {
                return Error.NotFound;
            }

            if (festival.IsExpired)
            {
                return FestivalError.FestivalIsExpired;
            }

            if (festival.IsDeleted)
            {
                return FestivalError.FestivalIsDeleted;
            }

            return Error.None;
        }

        private async Task<Error> ValidateFestivalEvents(List<int> eventList, int? id)
        {
            if (eventList.Count != eventList.Distinct().Count())
            {
                return FestivalError.EventDuplicates;
            }

            if (eventList.Count < 2)
            {
                return FestivalError.TooFewEventsInFestival;
            }

            if (eventList.Count > 12)
            {
                return FestivalError.TooMuchEventsInFestival;
            }

            var allEventsIdsInDB = (await _unitOfWork.GetRepository<Event>()
                                        .GetAllAsync(q => q.Where(e =>
                                            !e.IsDeleted &&
                                            e.EndDate > DateTime.Now))).Select(e => e.Id);

            Log.Information("EventList: {EventList}", string.Join(", ", eventList));
            Log.Information("AllEventsIdsInDB: {AllEventsIdsInDB}", string.Join(", ", allEventsIdsInDB));
            if (!eventList.All(eventId => allEventsIdsInDB.Contains(eventId)))
            {
                return EventError.EventNotFound;
            }

            var festivalEvents = (await _unitOfWork.GetRepository<Event>().GetAllAsync(q =>
                                        q.Where(e =>
                                            !e.IsDeleted &&
                                            e.EndDate > DateTime.Now &&
                                            eventList.Contains(e.Id)).OrderBy(e => e.StartDate))).ToList();

            if (festivalEvents.Last().EndDate - festivalEvents.First().StartDate > TimeSpan.FromDays(14))
            {
                return FestivalError.FestivalIsTooLong;
            }

            return ValidateFestivalEventsSequence(festivalEvents, id);
        }

        private Error ValidateFestivalEventsSequence(List<Event> festivalEvents, int? id)
        {
            for (var i = 0; i < festivalEvents.Count; i++)
            {
                var eventEntity = festivalEvents[i];
                if (eventEntity.IsDeleted)
                {
                    return EventError.EventIsDeleted;
                }

                if (eventEntity.IsExpired)
                {
                    return EventError.EventIsExpired;
                }

                if (i + 1 < festivalEvents.Count)
                {
                    var nextEventEntity = festivalEvents[i + 1];
                    if (nextEventEntity.StartDate - eventEntity.StartDate > TimeSpan.FromHours(48))
                    {
                        return FestivalError.TooMuchTimeBetweenEvents;
                    }
                }

                if (id == null && eventEntity.Festivals.Count() >= 3)
                {
                    return EventError.EventIsPartOfTooMuchFestivals;
                }
            }

            return Error.None;
        }

        private async Task<Error> ValidateFestivalTickets(ICollection<Event_FestivalTicketRequestDto> festivalTickets)
        {
            var ticketTypeIds = festivalTickets.Select(ft => ft.TicketTypeId);
            if (ticketTypeIds.Count() != ticketTypeIds.Distinct().Count())
            {
                return TicketTypeError.TicketDuplicates;
            }

            var allTicketTypes = await _unitOfWork.GetRepository<TicketType>().GetAllAsync(q => q.Where(tt => !tt.IsDeleted));
            var allTicketTypeIds = allTicketTypes.Select(tt => tt.Id);
            if (!ticketTypeIds.All(ticketTypeId => allTicketTypeIds.Contains(ticketTypeId)))
            {
                return TicketTypeError.NotFound;
            }

            var normalTicketTypeExist = ticketTypeIds.Any(typeId =>
                                    allTicketTypes.Any(tt =>
                                    tt.Name.Equals("Bilet normalny", StringComparison.OrdinalIgnoreCase) &&
                                    tt.Id == typeId));
            if (!normalTicketTypeExist)
            {
                return TicketTypeError.NormalTicketTypeNotFound;
            }

            return Error.None;
        }

        private async Task<Error> ValidateMediaPatrons(List<int> mediaPatronIds)
        {
            if (mediaPatronIds.Count != mediaPatronIds.Distinct().Count())
            {
                return FestivalError.MediaPatronDuplicates;
            }

            if (mediaPatronIds.Count > 10)
            {
                return FestivalError.TooMuchMediaPatronsInFestival;
            }

            var allMediaPatronsIdsInDB = (await _unitOfWork.GetRepository<Partner>().GetAllAsync(q => q.Where(mp => !mp.IsDeleted && mp.Kind == PartnerKind.MediaPatron))).Select(mp => mp.Id);
            if (!mediaPatronIds.All(mediaPatronId => allMediaPatronsIdsInDB.Contains(mediaPatronId)))
            {
                return PartnerError.MediaPatronNotFound;
            }

            return Error.None;
        }

        private async Task<Error> ValidateOrganizers(List<int> organizerIds)
        {
            if (organizerIds.Count != organizerIds.Distinct().Count())
            {
                return FestivalError.OrganizerDuplicates;
            }

            if (organizerIds.Count > 10)
            {
                return FestivalError.TooMuchOrganizersInFestival;
            }

            var allOrganizersIdsInDB = (await _unitOfWork.GetRepository<Partner>().GetAllAsync(q => q.Where(o => !o.IsDeleted && o.Kind == PartnerKind.Organizer))).Select(o => o.Id);
            if (!organizerIds.All(organizerId => allOrganizersIdsInDB.Contains(organizerId)))
            {
                return PartnerError.OrganizerNotFound;
            }

            return Error.None;
        }

        private async Task<Error> ValidateSponsors(List<int> sponsorIds)
        {
            if (sponsorIds.Count != sponsorIds.Distinct().Count())
            {
                return FestivalError.SponsorDuplicates;
            }

            if (sponsorIds.Count > 10)
            {
                return FestivalError.TooMuchSponsorsInFestival;
            }

            var allSponsorsIdsInDB = (await _unitOfWork.GetRepository<Partner>().GetAllAsync(q => q.Where(s => !s.IsDeleted && s.Kind == PartnerKind.Sponsor))).Select(s => s.Id);
            if (!sponsorIds.All(sponsorId => allSponsorsIdsInDB.Contains(sponsorId)))
            {
                return PartnerError.SponsorNotFound;
            }

            return Error.None;
        }

        private sealed record FestivalRelations(
            List<int> EventIds,
            List<int> MediaPatronIds,
            List<int> OrganizerIds,
            List<int> SponsorIds,
            ICollection<Event_FestivalTicketRequestDto> FestivalTickets);


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


        private IEnumerable<FestivalResponseDto> MapAsDto(IEnumerable<Festival> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.ToResponse();
                responseDto.Events = entity.Events.Select(e =>
                {
                    var eventEntity = e.ToResponse();
                    eventEntity.Category = e.Category.ToResponse();
                    eventEntity.LongDescription = null;
                    eventEntity.Hall = e.Hall.ToResponse();
                    eventEntity.Hall.Seats = [];
                    if (eventEntity.Hall.Type != null)
                        eventEntity.Hall.Type = null;
                    eventEntity.Tickets = [];
                    return eventEntity;
                }).ToList();
                responseDto.FestivalStatus = GetEntityStatus(entity);
                responseDto.Tickets = entity.Tickets.Select(t =>
                {
                    var ticket = t.ToResponse();
                    ticket.IsFestival = t.FestivalId.HasValue;
                    ticket.Event = null;
                    ticket.Festival = null;
                    ticket.TicketType = t.TicketType.ToResponse();
                    return ticket;
                }).ToList();
                responseDto.MediaPatrons = entity.Partners.Where(p => p.Kind == PartnerKind.MediaPatron).Select(p => _partnerService.MapAsDto(p)).ToList();
                responseDto.Organizers = entity.Partners.Where(p => p.Kind == PartnerKind.Organizer).Select(p => _partnerService.MapAsDto(p)).ToList();
                responseDto.Sponsors = entity.Partners.Where(p => p.Kind == PartnerKind.Sponsor).Select(p => _partnerService.MapAsDto(p)).ToList();
                responseDto.PhotoEndpoint = $"/Festivals/{responseDto.Id}/image";
                return responseDto;
            });
        }


        private FestivalResponseDto MapAsDto(Festival entity)
        {
            var responseDto = entity.ToResponse();
            responseDto.Events = entity.Events.Select(e =>
            {
                var eventEntity = e.ToResponse();
                eventEntity.Category = e.Category.ToResponse();
                eventEntity.LongDescription = null;
                eventEntity.Hall = e.Hall.ToResponse();
                eventEntity.Hall.Seats = [];
                if (eventEntity.Hall.Type != null)
                    eventEntity.Hall.Type = null;
                eventEntity.Tickets = [];
                return eventEntity;
            }).ToList();
            responseDto.FestivalStatus = GetEntityStatus(entity);
            responseDto.Tickets = entity.Tickets.Select(t =>
            {
                var ticket = t.ToResponse();
                ticket.IsFestival = t.FestivalId.HasValue;
                ticket.Event = null;
                ticket.Festival = null;
                ticket.TicketType = t.TicketType.ToResponse();
                return ticket;
            }).ToList();
            responseDto.MediaPatrons = entity.Partners.Where(p => p.Kind == PartnerKind.MediaPatron).Select(p => _partnerService.MapAsDto(p)).ToList();
            responseDto.Organizers = entity.Partners.Where(p => p.Kind == PartnerKind.Organizer).Select(p => _partnerService.MapAsDto(p)).ToList();
            responseDto.Sponsors = entity.Partners.Where(p => p.Kind == PartnerKind.Sponsor).Select(p => _partnerService.MapAsDto(p)).ToList();
            responseDto.PhotoEndpoint = $"/Festivals/{responseDto.Id}/image";
            return responseDto;
        }

        private Festival MapAsEntity(FestivalRequestDto requestDto) => requestDto.ToEntity();

        private IEntity MapToEntity(UpdateFestivalRequestDto requestDto, Festival oldEntity) => requestDto.ApplyTo(oldEntity);

        public async Task<Result<FestivalResponseDto>> GetOneAsync(int id)
        {
            if (id < 0)
                return Result<FestivalResponseDto>.Failure(Error.RouteParamOutOfRange);

            var record = await _repository.GetOneAsync(id);
            if (record == null)
                return Result<FestivalResponseDto>.Failure(Error.NotFound);

            var response = MapAsDto(record);

            return Result<FestivalResponseDto>.Success(response);
        }

        private async Task<Result<(Festival Entity, UserResponseDto User)>> ValidateBeforeDelete(int id)
        {
            if (id < 0)
                return Result<(Festival Entity, UserResponseDto User)>.Failure(Error.RouteParamOutOfRange);

            var entity = await _repository.GetOneAsync(id);
            if (entity == null)
                return Result<(Festival Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<(Festival Entity, UserResponseDto User)>.Failure(userResult.Error);

            if (entity.IsDeleted)
                return Result<(Festival Entity, UserResponseDto User)>.Failure(Error.NotFound);

            if (entity.IsExpired)
                return Result<(Festival Entity, UserResponseDto User)>.Failure(Error.EntityIsExpired);

            var user = userResult.Value;
            if (!user.IsInRole(Roles.Admin))
                return Result<(Festival Entity, UserResponseDto User)>.Failure(user.IsInRole(Roles.User) ? AuthError.UserDoesNotHavePremissionToResource : AuthError.UserDoesNotHaveSpecificRole);

            return Result<(Festival Entity, UserResponseDto User)>.Success((entity, user));
        }

        private Status GetEntityStatus(Festival entity)
        {
            if (!entity.IsDeleted && entity.EndDate > DateTime.Now) return Status.Active;
            else if (!entity.IsDeleted && entity.EndDate < DateTime.Now) return Status.Expired;
            else if (entity.IsDeleted) return Status.Canceled;

            return Status.Unknown;
        }
    }
}
