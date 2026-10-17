using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.DB.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFlowAPI.DB.Entities
{
    public class Reservation : BaseEntity, ISoftDeleteable, IExpireable, IAuthEntity, IDateableEntity
    {
        public Guid ReservationGuid { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }

        [NotMapped]
        public bool IsExpired => EndDate < DateTime.Now;

        [NotMapped]
        public bool IsFestivalReservation => Ticket?.FestivalId != null;

        [Range(0.00, 999.99),
         Column(TypeName = "NUMERIC(5,2)")]
        public decimal TotalAddtionalPaymentPercentage { get; set; }

        [Range(0.00, 9999.99),
        Column(TypeName = "NUMERIC(6,2)")]
        public decimal TotalAdditionalPaymentAmount { get; set; }

        [Range(0.00, 99999.99),
            Column(TypeName = "NUMERIC(7,2)")]
        public decimal TotalDiscount { get; set; }

        [Range(0.00, 99999.99),
         Column(TypeName = "NUMERIC(7,2)")]
        public decimal PaymentAmount { get; set; }   
        public string UserId { get; set; } = string.Empty;
        public PaymentType PaymentType { get; set; }
        public int TicketId { get; set; }
        public int? EventPassId { get; set; }
        public EventPass? EventPass { get; set; } = default!;
        public string? TicketPDFFileName { get; set; }
        public string? TicketImagesFolder { get; set; }
        public User User { get; set; } = default!;
        public Ticket Ticket { get; set; } = default!;
        public ICollection<Seat> Seats { get; set; } = [];
    }
}
