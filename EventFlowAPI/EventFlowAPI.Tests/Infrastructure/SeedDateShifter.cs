using EventFlowAPI.DB.Context;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Tests.Infrastructure
{
    public class SeedDateShifter
    {
        private static readonly DateTime SeedBaseDate = new(2026, 10, 12);

        private static readonly Dictionary<string, string[]> DateColumns = new()
        {
            ["Event"] = ["StartDate", "EndDate"],
            ["Festival"] = ["StartDate", "EndDate"],
            ["EventPass"] = ["StartDate", "EndDate", "PaymentDate"],
            ["HallRent"] = ["StartDate", "EndDate", "RentDate", "PaymentDate"],
            ["Reservation"] = ["ReservationDate", "StartDate", "EndDate", "PaymentDate"],
            ["News"] = ["PublicationDate"],
            ["Users"] = ["RegisteredDate", "DateOfBirth"],
        };

        public async Task ShiftToTodayAsync(APIContext context)
        {
            var days = (DateTime.Today - SeedBaseDate).Days;
            if (days == 0)
            {
                return;
            }

            foreach (var (table, columns) in DateColumns)
            {
                foreach (var column in columns)
                {
                    var sql = $"UPDATE [{table}] SET [{column}] = DATEADD(day, {days}, [{column}]) WHERE [{column}] IS NOT NULL";
                    await context.Database.ExecuteSqlRawAsync(sql);
                }
            }
        }
    }
}
