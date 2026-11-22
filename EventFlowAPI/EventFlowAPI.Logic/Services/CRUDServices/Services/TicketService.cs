using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using Serilog;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class TicketService(IUnitOfWork unitOfWork) : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITicketRepository _repository = unitOfWork.Tickets;
        /*public async Task UpdateTicketsForFestival(ICollection<Event_FestivalTicketRequestDto> newFestivalTickets, Festival festival)
        {
            ICollection<Ticket> updatedTickets = [];

            var oldTicketTypesIds = festival.Tickets.Select(t => t.TicketTypeId).ToList();

            foreach(var newTicket in newFestivalTickets)
            {
                if (oldTicketTypesIds.Contains(newTicket.TicketTypeId))
                {
                    var oldFestivalTickets = festival.Tickets.Where(t => t.TicketTypeId == newTicket.TicketTypeId);
                    foreach(var festivalTicket in oldFestivalTickets)
                    {
                        if (festivalTicket.Price != newTicket.Price)
                        {
                            festivalTicket.Price = newTicket.Price;
                            _repository.Update(festivalTicket);
                        }
                        updatedTickets.Add(festivalTicket);
                    }
                }
                else
                {
                    foreach(var eventEntity in festival.Events)
                    {       
                        var festivalTicket = new Ticket
                        {
                            Price = newTicket.Price,
                            TicketTypeId = newTicket.TicketTypeId,
                            EventId = eventEntity.Id,
                            FestivalId = festival.Id,
                        };
                        await _repository.AddAsync(festivalTicket);
                    }
                }
            }
            if (updatedTickets.Count() != festival.Tickets.Count())
            {
                var ticketsToDelete = festival.Tickets.Except(updatedTickets).ToList();
                foreach (var ticket in ticketsToDelete)
                {
                    if (ticket.Reservations.Any())
                    {
                        ticket.IsDeleted = true;
                        _repository.Update(ticket);
                    }
                    else
                    {
                        _repository.Delete(ticket);
                    }
                }
            }
        }*/

        public async Task UpdateTicketsForFestival(ICollection<Event_FestivalTicketRequestDto> newFestivalTickets, Festival festival)
        {
            Log.Information("Starting to update tickets for festival. Festival ID: {FestivalId}", festival.Id);

            ICollection<Ticket> updatedTickets = new List<Ticket>(); // Tworzymy pustą listę na zaktualizowane bilety
            ICollection<Ticket> addedTickets = new List<Ticket>(); // Tworzymy pustą listę na zaktualizowane bilety

            var oldTicketTypesIds = festival.Tickets.Select(t => t.TicketTypeId).ToList();
            Log.Information("Old ticket types for the festival: {OldTicketTypesIds}", string.Join(", ", oldTicketTypesIds));

            foreach (var newTicket in newFestivalTickets)
            {
                Log.Information("Processing new ticket with TicketTypeId: {TicketTypeId}, Price: {Price}", newTicket.TicketTypeId, newTicket.Price);

                if (oldTicketTypesIds.Contains(newTicket.TicketTypeId))
                {
                    // Zaktualizuj istniejące bilety
                    var oldFestivalTickets = festival.Tickets.Where(t => t.TicketTypeId == newTicket.TicketTypeId);
                    foreach (var festivalTicket in oldFestivalTickets)
                    {
                        if (festivalTicket.Price != newTicket.Price)
                        {
                            Log.Information("Updating ticket for TicketTypeId: {TicketTypeId} with old price {OldPrice} to new price {NewPrice}",
                                            newTicket.TicketTypeId, festivalTicket.Price, newTicket.Price);

                            festivalTicket.Price = newTicket.Price;
                            _repository.Update(festivalTicket); // Zaktualizowanie biletu w bazie danych
                        }
                        updatedTickets.Add(festivalTicket); // Dodanie biletu do zaktualizowanych
                    }
                }
                else
                {
                    // Dodaj nowy bilet, jeśli TicketTypeId nie istnieje
                    foreach (var eventEntity in festival.Events)
                    {
                        Log.Information("Adding new ticket for EventId: {EventId}, FestivalId: {FestivalId}, TicketTypeId: {TicketTypeId}, Price: {Price}",
                                        eventEntity.Id, festival.Id, newTicket.TicketTypeId, newTicket.Price);

                        var festivalTicket = new Ticket
                        {
                            Price = newTicket.Price,
                            TicketTypeId = newTicket.TicketTypeId,
                            EventId = eventEntity.Id,
                            FestivalId = festival.Id,
                        };
                        addedTickets.Add(festivalTicket);
                        await _repository.AddAsync(festivalTicket); // Dodanie nowego biletu do bazy danych
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            // Logowanie przed usuwaniem biletów, jeśli ilość zaktualizowanych biletów się różni
            if (updatedTickets.Count() != festival.Tickets.Count())
            {
                var ticketsToDelete = festival.Tickets.Except(updatedTickets.Union(addedTickets)).ToList();
                Log.Information("Tickets to delete (not updated): {TicketsToDelete}", string.Join(", ", ticketsToDelete.Select(t => t.Id)));

                foreach (var ticket in ticketsToDelete)
                {
                    if (ticket.Reservations.Any())
                    {
                        Log.Information("Marking ticket {TicketId} as deleted because it has reservations.", ticket.Id);
                        ticket.IsDeleted = true;
                        _repository.Update(ticket); // Zaktualizowanie biletu w bazie danych
                    }
                    else
                    {
                        Log.Information("Deleting ticket {TicketId} because it has no reservations.", ticket.Id);
                        _repository.Delete(ticket); // Usunięcie biletu z bazy danych
                    }
                }
            }        

            Log.Information("Finished updating tickets for festival. Festival ID: {FestivalId}", festival.Id);
        }

        public async Task UpdateTicketsForEvent(ICollection<Event_FestivalTicketRequestDto> newEventTickets, Event oldEvent)
        {
            ICollection<Ticket> updatedTickets = [];

            var oldTicketTypesIds = oldEvent.Tickets.Where(t => t.Festival == null).Select(t => t.TicketTypeId).ToList();
            foreach (var newTicket in newEventTickets)
            {
                if (oldTicketTypesIds.Contains(newTicket.TicketTypeId))
                {
                    var oldTicket = oldEvent.Tickets.Where(t =>
                                        t.Festival == null &&
                                        t.TicketTypeId == newTicket.TicketTypeId).First();

                    if (oldTicket.Price != newTicket.Price)
                    {
                        oldTicket.Price = newTicket.Price;
                        _repository.Update(oldTicket);
                    }
                    updatedTickets.Add(oldTicket);
                }
                else
                {
                    var eventTicket = new Ticket
                    {
                        Price = newTicket.Price,
                        TicketTypeId = newTicket.TicketTypeId,
                        EventId = oldEvent.Id,
                    };
                    await _repository.AddAsync(eventTicket);
                }
            }
            if (updatedTickets.Count() != oldEvent.Tickets.Where(t => t.Festival == null).Count())
            {
                var ticketsToDelete = oldEvent.Tickets.Where(t => t.Festival == null).Except(updatedTickets);
                foreach (var ticket in ticketsToDelete)
                {
                    if (ticket.Reservations.Any())
                    {
                        ticket.IsDeleted = true;
                        _repository.Update(ticket);
                    }
                    else
                    {
                        _repository.Delete(ticket);
                    }
                }
            }
        }

        public ICollection<Ticket> GetFestivalTickets(ICollection<Event_FestivalTicketRequestDto> ticketsDto, ICollection<Event> festivalEventList)
        {
            ICollection<Ticket> tickets = [];
            foreach (var ticket in ticketsDto)
            {
                foreach(var eventEntity in festivalEventList)
                {
                    var eventTicket = new Ticket
                    {
                        Price = ticket.Price,
                        TicketTypeId = ticket.TicketTypeId,
                        EventId = eventEntity.Id,
                    };
                    tickets.Add(eventTicket);
                }
            }
            return tickets;
        }

        public ICollection<Ticket> GetEventTickets(ICollection<Event_FestivalTicketRequestDto> ticketsDto)
        {
            ICollection<Ticket> tickets = [];
            foreach (var ticket in ticketsDto)
            {
                var eventTicket = new Ticket
                {
                    Price = ticket.Price,
                    TicketTypeId = ticket.TicketTypeId,
                };
                tickets.Add(eventTicket);
            }
            return tickets;
        }

        public async Task DeleteTickets(ICollection<Event> eventsToDelete, ICollection<Festival> festivalsToDelete)
        {
            var tickets = await _repository.GetForEventsOrFestivalsAsync(
                                   eventsToDelete.Select(e => e.Id),
                                   festivalsToDelete.Select(f => f.Id));

            // TODO (block 5): ticket.Reservations is filled only when the reservations were loaded earlier in the request

            foreach (var ticket in tickets)
            {
                if (ticket.Reservations.Any())
                {
                    ticket.IsDeleted = true;
                    _repository.Update(ticket);
                }
                else
                {
                    _repository.Delete(ticket);
                }
            }
        }
    }
}
