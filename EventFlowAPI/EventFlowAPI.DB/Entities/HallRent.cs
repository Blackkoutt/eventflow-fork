using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFlowAPI.DB.Entities
{
    public class HallRent : BaseEntity, ISoftDeleteable, IExpireable, IAuthEntity, IDateableEntity
    {
        public Guid HallRentGuid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long Duration { get; set; }
        public DateTime RentDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }

        [NotMapped]
        public bool IsExpired => EndDate < DateTime.Now;


        [NotMapped]
        public TimeSpan DurationTimeSpan
        {
            get => TimeSpan.FromSeconds(Duration);
            set => Duration = (long)value.TotalSeconds;
        }

        [Range(0.00, 99999.99),
         Column(TypeName = "NUMERIC(7,2)")]
        public decimal PaymentAmount { get; set; }

        public string? HallRentPDFName { get; set; }
        public int PaymentTypeId { get; set; }   
        public int HallId { get; set; }
        public string UserId { get; set; } = string.Empty;
        [NotMapped]
        public PaymentType PaymentType => PaymentTypes.All.First(paymentType => paymentType.Id == PaymentTypeId);
        public Hall Hall { get; set; } = default!;
        public User User { get; set; } = default!;
        public ICollection<AdditionalServices> AdditionalServices { get; set; } = [];

    }
}
