namespace EventFlowAPI.Logic.DTO.Statistics.ResponseDto
{
    public class StatisticsResponseDto
    {
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public TotalIncomeStatistics TotalIncome { get; set; } = default!;
        public HallRentStatistics? HallRentStatistics { get; set;}
        public EventStatistics? EventStatistics { get; set;}
        public EventPassStatistics? EventPassStatistics { get; set;}
        public FestivalStatistics? FestivalStatistics { get; set; }
        public ReservationStatistics? ReservationStatistics { get; set; }
        public PaymentStatistics? PaymentStatistics { get; set; }
        public UserStatistics? UserStatistics { get; set; }
    }
}
