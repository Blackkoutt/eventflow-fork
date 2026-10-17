using EventFlowAPI.Logic.DTO.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.Logic.DTO.RequestDto
{
    // rows and columns and max number of seats needs to be reduced 
    public class HallRequestDto : IRequestDto
    {
        [Required(ErrorMessage = "Numer sali jest wymagany.")]
        public int HallNr { get; set; }

        [Required(ErrorMessage = "Cena wynajmu sali jest wymagana.")]
        [Range(0.00, 999.99, ErrorMessage = "Cena nie może być mniejsza niż 0 lub większa niż 999.99.")]     
        public decimal RentalPricePerHour { get; set; }

        [Required(ErrorMessage = "Numer piętra na którym znajduje się sala jest wymagany.")]
        [Range(1,4, ErrorMessage = "Numer piętra nie może być mniejszy niż 0 lub większy niż 4.")]   
        public int Floor { get; set; }

        [Required(ErrorMessage = "Długość sali jest wymagana.")]
        [Range(4, 99.99, ErrorMessage = "Długość sali nie może być mniejsza niż 4 metry i większa niż 99.99 metrów.")]
        public decimal TotalLength { get; set; }

        [Required(ErrorMessage = "Szerokość sali jest wymagana.")]
        [Range(4, 99.99, ErrorMessage = "Szerokość sali nie może być mniejsza niż 4 metry i większa niż 99.99 metrów.")]
        public decimal TotalWidth { get; set; }

        [Range(3.00, 30.00, ErrorMessage = "Długość sceny nie może być mniejsza niż 3 m lub większa niż 30 m.")]
        public float? StageLength { get; set; }

        [Range(3.00, 30.00, ErrorMessage = "Szerokość sceny nie może być mniejsza niż 3 m lub większa niż 30 m.")]
        public float? StageWidth { get; set; }

        [Required(ErrorMessage = "Ilość rzędów miejsc w sali jest wymagana.")]
        [Range(1, 20, ErrorMessage = "Ilość rzędów miejsc w sali nie może być mniejsza niż 1 lub większa niż 20.")]
        public int MaxNumberOfSeatsRows { get; set; }

        [Required(ErrorMessage = "Ilość kolumn miejsc w sali jest wymagana.")]
        [Range(1, 20, ErrorMessage = "Ilość kolumn miejsc w sali nie może być mniejsza niż 1 lub większa niż 20.")]
        public int MaxNumberOfSeatsColumns { get; set; }


        [Required(ErrorMessage = "Typ sali jest wymagany.")]
        public int HallTypeId { get; set; }

        public ICollection<SeatRequestDto> Seats { get; set; } = [];
    }
}
