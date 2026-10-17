using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFlowAPI.DB.Entities
{
    public class EventPass : BaseEntity, ISoftDeleteable, IExpireable, IAuthEntity, IDateableEntity
    {
        public Guid EventPassGuid { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime? RenewalDate { get; set; } = null;
        public DateTime EndDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [NotMapped]
        public bool IsExpired => EndDate < DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public DateTime PaymentDate { get; set; }

        [Range(0.00, 9999.99),
         Column(TypeName = "NUMERIC(6,2)")]
        public decimal PaymentAmount { get; set; }

        [Range(0.00, 99.99),
            Column(TypeName = "NUMERIC(4,2)")]
        public decimal TotalDiscountPercentage { get; set; }

        [Range(0.00, 999.99),
            Column(TypeName = "NUMERIC(5,2)")]
        public decimal TotalDiscount { get; set; }
        public string? EventPassJPGName { get; set; } = string.Empty;
        public string? EventPassPDFName { get; set; } = string.Empty;
        public int PassTypeId { get; set; }
        public string UserId {  get; set; } = string.Empty;
        public int PaymentTypeId { get; set; }
        public EventPassType PassType { get; set; } = default!;
        public User User { get; set; } = default!;
        [NotMapped]
        public PaymentType PaymentType => PaymentTypes.All.First(paymentType => paymentType.Id == PaymentTypeId);
        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
