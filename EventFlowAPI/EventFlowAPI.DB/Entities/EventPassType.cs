using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFlowAPI.DB.Entities
{
    public class EventPassType : BaseEntity, INameableEntity, IPriceableEntity, ISoftDeleteable
    {

        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 99),
         Column(TypeName = "NUMERIC(2)")]
        public int ValidityPeriodInMonths { get; set; }

        [Range(0.00, 9999.99),
         Column(TypeName = "NUMERIC(6,2)")]
        public decimal Price { get; set; }

        [Range(0.00, 99.99),
        Column(TypeName = "NUMERIC(4,2)")]
        public decimal RenewalDiscountPercentage { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }
        public ICollection<EventPass> EventPasses { get; set; } = [];
    }
}
