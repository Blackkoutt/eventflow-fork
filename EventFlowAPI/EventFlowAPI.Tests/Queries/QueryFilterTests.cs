using System.Text.Json;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Queries
{
    public record Expectation(string Query, int Status, int[] Ids);

    public record QueryCase(string Role, string Route, Expectation[] Expectations);

    [Collection(ReadOnlyApiCollection.Name)]
    public class QueryFilterTests
    {
        private static Expectation Ok(string query, params int[] ids) => new(query, 200, ids);

        private static Expectation Fails(string query, int status) => new(query, status, Array.Empty<int>());

        // Expected ids of the seeded data, in the order the API returns them.
        // Queries with {d+N} / {d-N} are resolved to today +/- N days.
        private static readonly Dictionary<string, QueryCase> Cases = new()
        {
            ["Events"] = new("anonymous", "/api/Events", new[]
            {
                Ok("Status=Active", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("Status=Expired", 9, 10, 11, 12),
                Ok("Status=Canceled"),
                Ok("Status=Unknown", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
                Ok("Name=koncert", 1, 5),
                Ok("Name=KONCERT", 1, 5),
                Ok("Name=zzzz"),
                Ok("FromDate={d+30}", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("FromDate={d+30}&ToDate={d+40}", 1, 2, 3, 4),
                Ok("ToDate={d+20}", 9, 10, 11, 12),
                Ok("ToDate={d-5}"),
                Ok("MinDuration=01:00:00", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
                Ok("MaxDuration=01:00:00", 1, 5),
                Ok("MinDuration=01:00:00&MaxDuration=02:00:00", 1, 3, 5, 6, 9, 10, 11, 12),
                Ok("CategoryId=1", 1, 5, 9),
                Ok("CategoryId=3", 2, 8, 11),
                Ok("HallNr=2", 1, 5, 9),
                Ok("TicketTypeId=2", 2, 6),
                Ok("IsFestivalEvent=true", 1, 2, 4, 5, 6, 7),
                Ok("IsFestivalEvent=false", 3, 8, 9, 10, 11, 12),
                Ok("MinTicketPrice=30", 2, 6, 7),
                Ok("MaxTicketPrice=25", 1, 4, 5, 8),
                Ok("MinTicketPrice=25&MaxTicketPrice=30", 2, 3, 6),
                Ok("SortBy=Name", 8, 2, 6, 10, 9, 1, 5, 3, 7, 4, 12, 11),
                Ok("SortBy=Name&SortDirection=DESC", 11, 12, 4, 7, 3, 5, 1, 9, 10, 6, 2, 8),
                Ok("SortBy=StartDate&SortDirection=DESC", 8, 7, 6, 5, 4, 3, 2, 1, 9, 10, 11, 12),
                Ok("SortBy=Duration", 1, 5, 3, 6, 9, 10, 11, 12, 8, 2, 4, 7),
                Ok("SortBy=Unknown", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
                Ok("PageNumber=1&PageSize=5", 1, 2, 3, 4, 5),
                Ok("PageNumber=2&PageSize=5", 6, 7, 8, 9, 10),
                Ok("PageNumber=3&PageSize=5", 11, 12),
                Ok("PageNumber=1", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
                Ok("PageSize=5", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
                Ok("Status=Active&CategoryId=1&SortBy=StartDate&SortDirection=DESC&PageNumber=1&PageSize=2", 5, 1),
            }),
            ["Festivals"] = new("anonymous", "/api/Festivals", new[]
            {
                Ok("Status=Active", 1, 2, 3),
                Ok("Status=Expired"),
                Ok("Status=Canceled"),
                Ok("Name=festiwal", 1, 2, 3),
                Ok("Name=zzzz"),
                Ok("FromDate={d+30}", 1, 2, 3),
                Ok("FromDate={d+30}&ToDate={d+40}", 1, 2, 3),
                Ok("ToDate={d+20}"),
                Ok("MinDuration=00:00:01", 1, 2, 3),
                Ok("MaxDuration=00:00:01"),
                Ok("MinEventsCount=2", 1, 2, 3),
                Ok("MaxEventsCount=1"),
                Ok("MinEventsCount=3"),
                Ok("MediaPatronId=1", 1, 2),
                Ok("MediaPatronId=2", 1, 3),
                Ok("OrganizerId=1", 1, 2),
                Ok("OrganizerId=2", 1, 3),
                Ok("SponsorId=1", 1, 2),
                Ok("SponsorId=2", 1, 3),
                Ok("TicketTypeId=5", 1),
                Ok("MinTicketPrice=19", 1, 2),
                Ok("MaxTicketPrice=19"),
                Ok("MinTicketPrice=19&MaxTicketPrice=20", 1),
                Ok("SortBy=Name", 2, 1, 3),
                Ok("SortBy=Name&SortDirection=DESC", 3, 1, 2),
                Ok("SortBy=StartDate&SortDirection=DESC", 3, 2, 1),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3),
            }),
            ["Halls"] = new("anonymous", "/api/Halls", new[]
            {
                Ok("HallNr=2", 2),
                Ok("HallNr=99"),
                Ok("Floor=1", 2, 4),
                Ok("Floor=2", 1, 3),
                Ok("HallTypeId=1", 1),
                Ok("HallTypeId=2", 2),
                Ok("MinRentalPrice=100", 1, 3, 4),
                Ok("MaxRentalPrice=100", 2),
                Ok("MinRentalPrice=90&MaxRentalPrice=130", 1),
                Ok("MinSeatsCount=10", 1, 2, 3, 4),
                Ok("MaxSeatsCount=10"),
                Ok("MinHallArea=200", 1),
                Ok("MaxHallArea=200", 2, 3, 4),
                Ok("IsHallHaveStage=true", 1, 3, 4),
                Ok("IsHallHaveStage=false", 2),
                Ok("RentFromDate={d+32}&RentToDate={d+33}", 1, 2, 3, 4),
                Ok("RentFromDate={d+200}&RentToDate={d+201}", 1, 2, 3, 4),
                Ok("RentFromDate={d+32}", 1, 2, 3, 4),
                Ok("SortBy=HallNr&SortDirection=DESC", 4, 3, 2, 1),
                Ok("SortBy=RentalPricePerHour", 2, 1, 3, 4),
                Ok("SortBy=Floor&SortDirection=DESC", 1, 3, 2, 4),
                Ok("PageNumber=1&PageSize=5", 1, 2, 3, 4),
                Ok("PageNumber=2&PageSize=5"),
            }),
            ["Reservations_admin"] = new("admin", "/api/Reservations", new[]
            {
                Ok("Status=Active", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("Status=Canceled"),
                Ok("Status=Expired"),
                Ok("EventId=1", 1, 5),
                Ok("EventId=2", 2, 7),
                Ok("EventId=999"),
                Ok("FromReservationDate={d-1}", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("ToReservationDate={d-1}"),
                Ok("FromReservationDate={d+1}"),
                Ok("FromDate={d+30}", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("ToDate={d+20}"),
                Ok("IsFestivalReservation=true", 5, 6, 7, 8),
                Ok("IsFestivalReservation=false", 1, 2, 3, 4),
                Ok("MinPrice=30", 2),
                Ok("MaxPrice=30", 1, 3, 4, 5, 6, 7, 8),
                Ok("MinPrice=20&MaxPrice=40", 1, 2, 3, 7, 8),
                Fails("UserName=Mateusz2", 500),
                Fails("UserName=Mateusz2%20Strapczuk2", 500),
                Fails("UserName=zzzz", 500),
                Ok("PaymentTypeId=1", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("PaymentTypeId=2"),
                Ok("ReservationByEventPass=true"),
                Ok("ReservationByEventPass=false", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("TicketTypeId=1", 1, 2, 3, 4, 5, 6),
                Ok("TicketTypeId=2", 7, 8),
                Ok("MinSeatsCount=2"),
                Ok("MaxSeatsCount=1", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("SortBy=PaymentAmount", 4, 5, 6, 1, 3, 7, 8, 2),
                Ok("SortBy=PaymentAmount&SortDirection=DESC", 2, 3, 7, 8, 1, 4, 5, 6),
                Ok("SortBy=ReservationDate&SortDirection=DESC", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("PageNumber=1&PageSize=3", 1, 2, 3),
                Ok("PageNumber=2&PageSize=3", 4, 5, 6),
                Ok("GetAll=true", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("GetAll=true&Status=Active", 1, 2, 3, 4, 5, 6, 7, 8),
                Ok("GetAll=false", 1, 2, 3, 4, 5, 6, 7, 8),
            }),
            ["Reservations_user2"] = new("user2", "/api/Reservations", new[]
            {
                Ok("Status=Active", 2, 5, 6, 7, 8),
                Ok("MinPrice=30", 2),
                Ok("SortBy=PaymentAmount&SortDirection=DESC", 2, 7, 8, 5, 6),
                Ok("PageNumber=1&PageSize=2", 2, 5),
                Ok("GetAll=true&PageNumber=1&PageSize=3", 1, 2, 3),
            }),
            ["HallRents_admin"] = new("admin", "/api/HallRents", new[]
            {
                Ok("Status=Active", 1, 2, 3, 4),
                Ok("Status=Canceled"),
                Ok("Status=Expired"),
                Ok("FromDate={d+30}", 1, 2, 3, 4),
                Ok("ToDate={d+40}", 1, 2, 3, 4),
                Ok("FromRentDate={d-1}", 1, 2, 3, 4),
                Ok("ToRentDate={d-1}"),
                Ok("FromRentDate={d+1}"),
                Ok("MinDuration=01:00:00", 1, 2, 3, 4),
                Ok("MaxDuration=01:00:00", 4),
                Ok("MinPrice=500", 1, 2),
                Ok("MaxPrice=500", 3, 4),
                Ok("PaymentTypeId=1", 1, 2, 3, 4),
                Ok("PaymentTypeId=2"),
                Ok("HallNr=2", 2),
                Ok("HallNr=99"),
                Ok("UserName=Mateusz3", 2, 3),
                Ok("UserName=zzzz"),
                Ok("SortBy=PaymentAmount&SortDirection=DESC", 1, 2, 3, 4),
                Ok("SortBy=StartDate", 1, 2, 3, 4),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3, 4),
                Ok("GetAll=true", 1, 2, 3, 4),
            }),
            ["HallRents_user3"] = new("user3", "/api/HallRents", new[]
            {
                Ok("Status=Active", 2, 3),
                Ok("MinPrice=100", 2, 3),
                Ok("SortBy=PaymentAmount&SortDirection=DESC", 2, 3),
                Ok("PageNumber=1&PageSize=1", 2),
            }),
            ["EventPasses_admin"] = new("admin", "/api/EventPasses", new[]
            {
                Ok("Status=Active", 1, 2, 3),
                Ok("Status=Canceled"),
                Ok("Status=Expired"),
                Ok("FromDate={d-1}", 1, 2, 3),
                Ok("ToDate={d+1}", 1, 2, 3),
                Ok("IsRenewed=true"),
                Ok("IsRenewed=false", 1, 2, 3),
                Ok("MinPrice=100", 1, 2, 3),
                Ok("MaxPrice=100"),
                Ok("PaymentTypeId=1", 1, 2, 3),
                Ok("PassTypeId=1"),
                Ok("PassTypeId=2", 3),
                Ok("PassTypeId=999"),
                Fails("UserName=Mateusz2", 500),
                Fails("UserName=zzzz", 500),
                Ok("SortBy=PaymentAmount&SortDirection=DESC", 2, 1, 3),
                Ok("SortBy=StartDate", 1, 2, 3),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3),
                Ok("GetAll=true", 1, 2, 3),
            }),
            ["Users_admin"] = new("admin", "/api/Users", new[]
            {
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3, 4),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3, 4, 5),
                Ok("PageSize=2", 1, 2, 3, 4, 5),
            }),
            ["News"] = new("anonymous", "/api/News", new[]
            {
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3, 4),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3, 4, 5, 6),
                Ok("PageSize=2", 1, 2, 3, 4, 5, 6),
                Ok("SortBy=Title", 1, 3, 5, 2, 6, 4),
                Ok("SortBy=Title&SortDirection=DESC", 4, 6, 2, 5, 3, 1),
                Ok("SortBy=Id&SortDirection=DESC", 6, 5, 4, 3, 2, 1),
            }),
            ["FAQ"] = new("anonymous", "/api/FAQ", new[]
            {
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3, 4),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3, 4),
                Ok("PageSize=2", 1, 2, 3, 4),
                Ok("SortBy=Question", 3, 2, 4, 1),
                Ok("SortBy=Id&SortDirection=DESC", 4, 3, 2, 1),
            }),
            ["Partners"] = new("anonymous", "/api/Partners", new[]
            {
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3, 4),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3, 4, 5),
                Ok("PageSize=2", 1, 2, 3, 4, 5),
                Ok("SortBy=Name", 2, 1, 4, 3, 5),
                Ok("SortBy=Id&SortDirection=DESC", 5, 4, 3, 2, 1),
            }),
            ["AdditionalServices"] = new("anonymous", "/api/AdditionalServices", new[]
            {
                Ok("Name=a", 2, 3, 4, 5, 6, 7),
                Ok("Name=A", 2, 3, 4, 5, 6, 7),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 7, 1, 4, 6, 3, 2, 5),
                Ok("SortBy=Name&SortDirection=DESC", 5, 2, 3, 6, 4, 1, 7),
                Ok("SortBy=Id&SortDirection=DESC", 7, 6, 5, 4, 3, 2, 1),
                Ok("SortBy=Unknown", 1, 2, 3, 4, 5, 6, 7),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 5, 2),
                Ok("MinPrice=20", 1, 2, 3, 4, 5, 6, 7),
                Ok("MaxPrice=20"),
                Ok("MinPrice=10&MaxPrice=50"),
                Ok("MinPrice=1000", 7),
                Ok("SortBy=Price&SortDirection=DESC", 7, 6, 1, 2, 3, 4, 5),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3, 4),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3, 4, 5, 6, 7),
                Ok("PageSize=2", 1, 2, 3, 4, 5, 6, 7),
            }),
            ["EventPassTypes"] = new("anonymous", "/api/EventPassTypes", new[]
            {
                Ok("Name=a", 1, 2, 3, 4),
                Ok("Name=A", 1, 2, 3, 4),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 2, 1, 3, 4),
                Ok("SortBy=Name&SortDirection=DESC", 4, 3, 1, 2),
                Ok("SortBy=Id&SortDirection=DESC", 4, 3, 2, 1),
                Ok("SortBy=Unknown", 1, 2, 3, 4),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 4, 3),
                Ok("MinPrice=20", 1, 2, 3, 4),
                Ok("MaxPrice=20"),
                Ok("MinPrice=10&MaxPrice=50"),
                Ok("MinPrice=1000"),
                Ok("SortBy=Price&SortDirection=DESC", 4, 3, 2, 1),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3, 4),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3, 4),
                Ok("PageSize=2", 1, 2, 3, 4),
            }),
            ["Equipments"] = new("anonymous", "/api/Equipments", new[]
            {
                Ok("Name=a", 1, 3, 4, 6, 7, 9, 10, 11),
                Ok("Name=A", 1, 3, 4, 6, 7, 9, 10, 11),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 7, 9, 5, 3, 4, 2, 1, 11, 8, 6, 10),
                Ok("SortBy=Name&SortDirection=DESC", 10, 6, 8, 11, 1, 2, 4, 3, 5, 9, 7),
                Ok("SortBy=Id&SortDirection=DESC", 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1),
                Ok("SortBy=Unknown", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 10, 6),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3, 4),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11),
                Ok("PageSize=2", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11),
            }),
            ["EventCategories"] = new("anonymous", "/api/EventCategories", new[]
            {
                Ok("Name=a", 3, 4),
                Ok("Name=A", 3, 4),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 2, 1, 3, 4),
                Ok("SortBy=Name&SortDirection=DESC", 4, 3, 1, 2),
                Ok("SortBy=Id&SortDirection=DESC", 4, 3, 2, 1),
                Ok("SortBy=Unknown", 1, 2, 3, 4),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 4, 3),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3, 4),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3, 4),
                Ok("PageSize=2", 1, 2, 3, 4),
            }),
            ["HallTypes"] = new("anonymous", "/api/HallTypes", new[]
            {
                Ok("Name=a", 1, 2, 3, 4, 5, 6),
                Ok("Name=A", 1, 2, 3, 4, 5, 6),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 1, 2, 6, 5, 3, 4),
                Ok("SortBy=Name&SortDirection=DESC", 4, 3, 5, 6, 2, 1),
                Ok("SortBy=Id&SortDirection=DESC", 6, 5, 4, 3, 2, 1),
                Ok("SortBy=Unknown", 1, 2, 3, 4, 5, 6),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 4, 3),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3, 4),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3, 4, 5, 6),
                Ok("PageSize=2", 1, 2, 3, 4, 5, 6),
            }),
            ["MediaPatrons"] = new("anonymous", "/api/MediaPatrons", new[]
            {
                Ok("Name=a", 1, 2),
                Ok("Name=A", 1, 2),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 1, 2, 3),
                Ok("SortBy=Name&SortDirection=DESC", 3, 2, 1),
                Ok("SortBy=Id&SortDirection=DESC", 3, 2, 1),
                Ok("SortBy=Unknown", 1, 2, 3),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 2, 1),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3),
                Ok("PageSize=2", 1, 2, 3),
            }),
            ["Organizers"] = new("anonymous", "/api/Organizers", new[]
            {
                Ok("Name=a", 2, 3),
                Ok("Name=A", 2, 3),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 3, 1, 2),
                Ok("SortBy=Name&SortDirection=DESC", 2, 1, 3),
                Ok("SortBy=Id&SortDirection=DESC", 3, 2, 1),
                Ok("SortBy=Unknown", 1, 2, 3),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 2, 3),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3),
                Ok("PageSize=2", 1, 2, 3),
            }),
            ["PaymentTypes"] = new("anonymous", "/api/PaymentTypes", new[]
            {
                Ok("Name=a", 1, 2),
                Ok("Name=A", 1, 2),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 2, 1),
                Ok("SortBy=Name&SortDirection=DESC", 1, 2),
                Ok("SortBy=Id&SortDirection=DESC", 2, 1),
                Ok("SortBy=Unknown", 1, 2),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2"),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2),
                Ok("PageSize=2", 1, 2),
            }),
            ["SeatTypes"] = new("anonymous", "/api/SeatTypes", new[]
            {
                Ok("Name=a", 2),
                Ok("Name=A", 2),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 2, 1, 3),
                Ok("SortBy=Name&SortDirection=DESC", 3, 1, 2),
                Ok("SortBy=Id&SortDirection=DESC", 3, 2, 1),
                Ok("SortBy=Unknown", 1, 2, 3),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 2),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3),
                Ok("PageSize=2", 1, 2, 3),
            }),
            ["Sponsors"] = new("anonymous", "/api/Sponsors", new[]
            {
                Ok("Name=a", 1, 3),
                Ok("Name=A", 1, 3),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 1, 2, 3),
                Ok("SortBy=Name&SortDirection=DESC", 3, 2, 1),
                Ok("SortBy=Id&SortDirection=DESC", 3, 2, 1),
                Ok("SortBy=Unknown", 1, 2, 3),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 3, 1),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3),
                Ok("PageSize=2", 1, 2, 3),
            }),
            ["TicketTypes"] = new("anonymous", "/api/TicketTypes", new[]
            {
                Ok("Name=a", 1),
                Ok("Name=A", 1),
                Ok("Name=zzzz"),
                Ok("SortBy=Name", 1, 3, 2),
                Ok("SortBy=Name&SortDirection=DESC", 2, 3, 1),
                Ok("SortBy=Id&SortDirection=DESC", 3, 2, 1),
                Ok("SortBy=Unknown", 1, 2, 3),
                Ok("Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2", 1),
                Ok("PageNumber=1&PageSize=2", 1, 2),
                Ok("PageNumber=2&PageSize=2", 3),
                Ok("PageNumber=99&PageSize=2"),
                Ok("PageNumber=1", 1, 2, 3),
                Ok("PageSize=2", 1, 2, 3),
            }),
        };

        public static IEnumerable<object[]> Names
        {
            get
            {
                foreach (var name in Cases.Keys)
                {
                    yield return new object[] { name };
                }
            }
        }

        private readonly ApiFactory _factory;

        public QueryFilterTests(ApiFactory factory)
        {
            _factory = factory;
        }

        private string ResolveDates(string query)
        {
            var result = query;
            foreach (var offset in new[] { -5, -1, 1, 20, 30, 32, 33, 40, 200, 201 })
            {
                var token = offset < 0 ? $"{{d{offset}}}" : $"{{d+{offset}}}";
                result = result.Replace(token, DateTime.Today.AddDays(offset).ToString("yyyy-MM-dd"));
            }
            return result;
        }

        private async Task<(int Status, int[] Ids)> GetAsync(ApiClient client, string route, string query)
        {
            var response = await client.GetAsync($"{route}?{ResolveDates(query)}");
            var status = (int)response.StatusCode;
            if (status != 200)
            {
                return (status, Array.Empty<int>());
            }

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            return (status, json.EnumerateArray().Select(item => item.GetProperty("id").GetId()).ToArray());
        }

        [Fact]
        [Trait(TestCategories.Name, TestCategories.DatabaseSpecific)]
        public async Task Page_number_zero_currently_fails_with_500_on_every_paged_list()
        {
            var admin = await _factory.CreateClientForRoleAsync("admin");
            var failures = new List<string>();

            foreach (var queryCase in Cases.Values)
            {
                var response = await admin.GetAsync($"{queryCase.Route}?PageNumber=0&PageSize=2");
                if ((int)response.StatusCode != 500)
                {
                    failures.Add($"{queryCase.Route} returned {(int)response.StatusCode}");
                }
            }

            failures.ShouldBeEmpty();
        }

        [Theory]
        [MemberData(nameof(Names))]
        public async Task Filters_sorting_and_paging_return_expected_ids(string name)
        {
            var queryCase = Cases[name];
            var client = await _factory.CreateClientForRoleAsync(queryCase.Role);
            var failures = new List<string>();

            foreach (var expected in queryCase.Expectations)
            {
                var (status, ids) = await GetAsync(client, queryCase.Route, expected.Query);
                if (status != expected.Status || !ids.SequenceEqual(expected.Ids))
                {
                    failures.Add($"{expected.Query}: expected {expected.Status} [{string.Join(",", expected.Ids)}], got {status} [{string.Join(",", ids)}]");
                }
            }

            failures.ShouldBeEmpty();
        }
    }
}
