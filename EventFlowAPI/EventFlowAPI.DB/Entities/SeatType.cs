using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFlowAPI.DB.Entities
{
    public class SeatType : BaseEntity, INameableEntity, ISoftDeleteable
    {

        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(400)]
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }

        [Range(0.00, 99.99),
         Column(TypeName = "NUMERIC(4,2)")]
        public decimal AddtionalPaymentPercentage { get; set; }
        public string SeatColor { get; set; } = string.Empty;
        public ICollection<Seat> Seats { get; set; } = [];
    }
}
