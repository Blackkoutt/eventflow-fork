using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFlowAPI.DB.Entities
{
    public class Seat : BaseEntity
    {

        [Range(0, 999),
         Column(TypeName = "NUMERIC(3)")]
        public int SeatNr { get; set; }

        [Range(0, 99),
         Column(TypeName = "NUMERIC(2)")]
        public int Row { get; set; }

        [Range(0, 99),
         Column(TypeName = "NUMERIC(2)")]
        public int GridRow { get; set; }

        [Range(0, 99),
         Column(TypeName = "NUMERIC(2)")]
        public int Column { get; set; }

        [Range(0, 99),
         Column(TypeName = "NUMERIC(2)")]
        public int GridColumn { get; set; }

        public int SeatTypeId { get; set; }
        public int HallId { get; set; }
        public SeatType SeatType { get; set; } = default!;
        public Hall Hall { get; set; } = default!;
        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
