using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFlowAPI.DB.Entities
{
    public class Hall : BaseEntity, IVisibleEntity, ICopyableEntity
    {
        public int? DefaultId { get; set; }
        public int HallNr { get; set; }

        [Range(0.00, 999.99),
         Column(TypeName = "NUMERIC(5,2)")]
        public decimal RentalPricePerHour { get; set; }

        public bool IsCopy { get; set; } = false;

        public bool IsVisible { get; set; } = true;

        [Range(0,4),
         Column(TypeName = "NUMERIC(1)")]
        public int Floor { get; set; }
        public string? HallViewFileName { get; set; }
        public int HallTypeId { get; set; }   

        public HallDetails? HallDetails { get; set; }
        public HallType Type { get; set; } = default!;

        public ICollection<Event> Events { get; set; } = [];
        public ICollection<Seat> Seats { get; set; } = [];
        public ICollection<HallRent> Rents { get; set; } = [];
    }
}
