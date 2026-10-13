using System.Text;
using System.Text.Json;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Queries
{
    public record QueryCase(string Role, string Route, string[] Queries);

    [Collection(ReadOnlyApiCollection.Name)]
    public class QueryFilterTests
    {
        private static readonly string[] Paging =
        {
            "PageNumber=1&PageSize=2", "PageNumber=2&PageSize=2", "PageNumber=99&PageSize=2", "PageNumber=1", "PageSize=2",
        };

        private static readonly string[] NameQueries =
        {
            "Name=a", "Name=A", "Name=zzzz", "SortBy=Name", "SortBy=Name&SortDirection=DESC", "SortBy=Id&SortDirection=DESC", "SortBy=Unknown",
            "Name=a&SortBy=Name&SortDirection=DESC&PageNumber=1&PageSize=2",
        };

        private static readonly string[] PriceQueries =
        {
            "MinPrice=20", "MaxPrice=20", "MinPrice=10&MaxPrice=50", "MinPrice=1000", "SortBy=Price&SortDirection=DESC",
        };

        private static readonly string[] EventQueries =
        {
            "Status=Active", "Status=Expired", "Status=Canceled", "Status=Unknown", "Name=koncert", "Name=KONCERT", "Name=zzzz",
            "FromDate={d+30}", "FromDate={d+30}&ToDate={d+40}", "ToDate={d+20}", "ToDate={d-5}",
            "MinDuration=01:00:00", "MaxDuration=01:00:00", "MinDuration=01:00:00&MaxDuration=02:00:00",
            "CategoryId=1", "CategoryId=3", "HallNr=2", "TicketTypeId=2", "IsFestivalEvent=true", "IsFestivalEvent=false",
            "MinTicketPrice=30", "MaxTicketPrice=25", "MinTicketPrice=25&MaxTicketPrice=30",
            "SortBy=Name", "SortBy=Name&SortDirection=DESC", "SortBy=StartDate&SortDirection=DESC", "SortBy=Duration", "SortBy=Unknown",
            "PageNumber=1&PageSize=5", "PageNumber=2&PageSize=5", "PageNumber=3&PageSize=5", "PageNumber=1", "PageSize=5",
            "Status=Active&CategoryId=1&SortBy=StartDate&SortDirection=DESC&PageNumber=1&PageSize=2",
        };

        private static readonly string[] FestivalQueries =
        {
            "Status=Active", "Status=Expired", "Status=Canceled", "Name=festiwal", "Name=zzzz",
            "FromDate={d+30}", "FromDate={d+30}&ToDate={d+40}", "ToDate={d+20}",
            "MinDuration=00:00:01", "MaxDuration=00:00:01",
            "MinEventsCount=2", "MaxEventsCount=1", "MinEventsCount=3",
            "MediaPatronId=1", "MediaPatronId=2", "OrganizerId=1", "OrganizerId=2", "SponsorId=1", "SponsorId=2",
            "TicketTypeId=5", "MinTicketPrice=19", "MaxTicketPrice=19", "MinTicketPrice=19&MaxTicketPrice=20",
            "SortBy=Name", "SortBy=Name&SortDirection=DESC", "SortBy=StartDate&SortDirection=DESC",
            "PageNumber=1&PageSize=2", "PageNumber=2&PageSize=2",
        };

        private static readonly string[] HallQueries =
        {
            "HallNr=2", "HallNr=99", "Floor=1", "Floor=2", "HallTypeId=1", "HallTypeId=2",
            "MinRentalPrice=100", "MaxRentalPrice=100", "MinRentalPrice=90&MaxRentalPrice=130",
            "MinSeatsCount=10", "MaxSeatsCount=10", "MinHallArea=200", "MaxHallArea=200",
            "IsHallHaveStage=true", "IsHallHaveStage=false",
            "RentFromDate={d+32}&RentToDate={d+33}", "RentFromDate={d+200}&RentToDate={d+201}", "RentFromDate={d+32}",
            "SortBy=HallNr&SortDirection=DESC", "SortBy=RentalPricePerHour", "SortBy=Floor&SortDirection=DESC",
            "PageNumber=1&PageSize=5", "PageNumber=2&PageSize=5",
        };

        private static readonly string[] ReservationQueries =
        {
            "Status=Active", "Status=Canceled", "Status=Expired", "EventId=1", "EventId=2", "EventId=999",
            "FromReservationDate={d-1}", "ToReservationDate={d-1}", "FromReservationDate={d+1}",
            "FromDate={d+30}", "ToDate={d+20}", "IsFestivalReservation=true", "IsFestivalReservation=false",
            "MinPrice=30", "MaxPrice=30", "MinPrice=20&MaxPrice=40", "UserName=Mateusz2", "UserName=Mateusz2%20Strapczuk2", "UserName=zzzz",
            "PaymentTypeId=1", "PaymentTypeId=2", "ReservationByEventPass=true", "ReservationByEventPass=false",
            "TicketTypeId=1", "TicketTypeId=2", "MinSeatsCount=2", "MaxSeatsCount=1",
            "SortBy=PaymentAmount", "SortBy=PaymentAmount&SortDirection=DESC", "SortBy=ReservationDate&SortDirection=DESC",
            "PageNumber=1&PageSize=3", "PageNumber=2&PageSize=3",
            "GetAll=true", "GetAll=true&Status=Active", "GetAll=false",
        };

        private static readonly string[] UserReservationQueries =
        {
            "Status=Active", "MinPrice=30", "SortBy=PaymentAmount&SortDirection=DESC", "PageNumber=1&PageSize=2", "GetAll=true&PageNumber=1&PageSize=3",
        };

        private static readonly string[] HallRentQueries =
        {
            "Status=Active", "Status=Canceled", "Status=Expired", "FromDate={d+30}", "ToDate={d+40}",
            "FromRentDate={d-1}", "ToRentDate={d-1}", "FromRentDate={d+1}",
            "MinDuration=01:00:00", "MaxDuration=01:00:00", "MinPrice=500", "MaxPrice=500", "PaymentTypeId=1", "PaymentTypeId=2",
            "HallNr=2", "HallNr=99", "UserName=Mateusz3", "UserName=zzzz",
            "SortBy=PaymentAmount&SortDirection=DESC", "SortBy=StartDate", "PageNumber=1&PageSize=2", "PageNumber=2&PageSize=2",
            "GetAll=true",
        };

        private static readonly string[] UserHallRentQueries =
        {
            "Status=Active", "MinPrice=100", "SortBy=PaymentAmount&SortDirection=DESC", "PageNumber=1&PageSize=1",
        };

        private static readonly string[] EventPassQueries =
        {
            "Status=Active", "Status=Canceled", "Status=Expired", "FromDate={d-1}", "ToDate={d+1}",
            "IsRenewed=true", "IsRenewed=false", "MinPrice=100", "MaxPrice=100", "PaymentTypeId=1", "PassTypeId=1", "PassTypeId=2", "PassTypeId=999",
            "UserName=Mateusz2", "UserName=zzzz", "SortBy=PaymentAmount&SortDirection=DESC", "SortBy=StartDate",
            "PageNumber=1&PageSize=2", "PageNumber=2&PageSize=2", "GetAll=true",
        };

        private static readonly string[] LookupQueries = NameQueries.Concat(Paging).ToArray();

        private static readonly string[] PricedLookupQueries = NameQueries.Concat(PriceQueries).Concat(Paging).ToArray();

        private static readonly Dictionary<string, QueryCase> Cases = new()
        {
            ["Events"] = new("anonymous", "/api/Events", EventQueries),
            ["Festivals"] = new("anonymous", "/api/Festivals", FestivalQueries),
            ["Halls"] = new("anonymous", "/api/Halls", HallQueries),
            ["Reservations_admin"] = new("admin", "/api/Reservations", ReservationQueries),
            ["Reservations_user2"] = new("user2", "/api/Reservations", UserReservationQueries),
            ["HallRents_admin"] = new("admin", "/api/HallRents", HallRentQueries),
            ["HallRents_user3"] = new("user3", "/api/HallRents", UserHallRentQueries),
            ["EventPasses_admin"] = new("admin", "/api/EventPasses", EventPassQueries),
            ["Users_admin"] = new("admin", "/api/Users", Paging),
            ["News"] = new("anonymous", "/api/News", Paging.Concat(new[] { "SortBy=Title", "SortBy=Title&SortDirection=DESC", "SortBy=Id&SortDirection=DESC" }).ToArray()),
            ["FAQ"] = new("anonymous", "/api/FAQ", Paging.Concat(new[] { "SortBy=Question", "SortBy=Id&SortDirection=DESC" }).ToArray()),
            ["Partners"] = new("anonymous", "/api/Partners", Paging.Concat(new[] { "SortBy=Name", "SortBy=Id&SortDirection=DESC" }).ToArray()),
            ["AdditionalServices"] = new("anonymous", "/api/AdditionalServices", PricedLookupQueries),
            ["EventPassTypes"] = new("anonymous", "/api/EventPassTypes", PricedLookupQueries),
            ["Equipments"] = new("anonymous", "/api/Equipments", LookupQueries),
            ["EventCategories"] = new("anonymous", "/api/EventCategories", LookupQueries),
            ["HallTypes"] = new("anonymous", "/api/HallTypes", LookupQueries),
            ["MediaPatrons"] = new("anonymous", "/api/MediaPatrons", LookupQueries),
            ["Organizers"] = new("anonymous", "/api/Organizers", LookupQueries),
            ["PaymentTypes"] = new("anonymous", "/api/PaymentTypes", LookupQueries),
            ["SeatTypes"] = new("anonymous", "/api/SeatTypes", LookupQueries),
            ["Sponsors"] = new("anonymous", "/api/Sponsors", LookupQueries),
            ["TicketTypes"] = new("anonymous", "/api/TicketTypes", LookupQueries),
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

        private async Task<string> DescribeAsync(ApiClient client, string route, string query)
        {
            var response = await client.GetAsync($"{route}?{ResolveDates(query)}");
            var status = (int)response.StatusCode;
            if (status != 200)
            {
                return $"{query} => {status}";
            }

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            var ids = new List<string>();
            foreach (var item in json.EnumerateArray())
            {
                ids.Add(item.GetProperty("id").GetRawText().Trim('"'));
            }
            return $"{query} => {status} [{string.Join(",", ids)}]";
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
        public async Task Filters_sorting_and_paging_match_snapshot(string name)
        {
            var queryCase = Cases[name];
            var client = await _factory.CreateClientForRoleAsync(queryCase.Role);
            var lines = new StringBuilder();

            foreach (var query in queryCase.Queries)
            {
                lines.AppendLine(await DescribeAsync(client, queryCase.Route, query));
            }

            await Verifier.Verify(lines.ToString()).UseDirectory("QuerySnapshots").UseTextForParameters(name);
        }
    }
}
