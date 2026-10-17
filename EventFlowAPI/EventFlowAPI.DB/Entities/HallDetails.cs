using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.DB.Entities
{
    public class HallDetails : BaseEntity
    {
        [Range(0.00, 99.99),
        Column(TypeName = "NUMERIC(4,2)")]
        public decimal TotalLength { get; set; }

        [Range(0.00, 99.99),
         Column(TypeName = "NUMERIC(4,2)")]
        public decimal TotalWidth { get; set; }

        [NotMapped]
        public decimal TotalArea => Math.Round(TotalLength * TotalWidth, 2);

        [Range(10.00, 30.00),
            Column(TypeName = "NUMERIC(4,2)")]
        public float? StageLength { get; set; }

        [Range(10.00, 30.00),
            Column(TypeName = "NUMERIC(4,2)")]
        public float? StageWidth { get; set; }

       /* [Range(0, 99),
         Column(TypeName = "NUMERIC(2)")]
        public int NumberOfSeatsRows { get; set; }*/

        [Range(0, 99),
         Column(TypeName = "NUMERIC(2)")]
        public int MaxNumberOfSeatsRows { get; set; }

      /*  [Range(0, 99),
         Column(TypeName = "NUMERIC(2)")]
        public int NumberOfSeatsColumns { get; set; }*/

        [Range(0, 99),
         Column(TypeName = "NUMERIC(2)")]
        public int MaxNumberOfSeatsColumns { get; set; }

        [Range(0, 999),
            Column(TypeName = "NUMERIC(3)")]
        public int NumberOfSeats { get; set; }

        [Range(0, 999),
         Column(TypeName = "NUMERIC(3)")]
        public int MaxNumberOfSeats { get; set; }

        public Hall? Hall { get; set; }
    }
}
