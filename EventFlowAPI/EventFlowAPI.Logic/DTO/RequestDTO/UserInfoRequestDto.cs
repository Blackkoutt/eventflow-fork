using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.Validators;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.Logic.DTO.RequestDto
{
    public class UserInfoRequestDto : IRequestDto
    {
        [OptionalLengthValidator(2, 50, ErrorMessage = "Nazwa ulicy powinna zawierać od 2 do 50 znaków.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ0-9][A-Za-ząćęłńóśźż0-9' \-]*$", ErrorMessage = "Nazwa ulicy powinna zaczynać się wielką literą lub cyfrą, a także zawierać tylko litery, cyfry, myślniki, apostrofy oraz spacje.")]
        public string? Street { get; set; } = string.Empty;

        [Range(1, 999, ErrorMessage = "Numer domu nie może być mniejszy niż 1 lub większa niż 999.")]
        public int? HouseNumber { get; set; }

        [Range(1, 999, ErrorMessage = "Numer mieszkania nie może być mniejszy niż 1 lub większa niż 999.")]
        public int? FlatNumber { get; set; }

        [OptionalLengthValidator(2, 50, ErrorMessage = "Nazwa miasta powinna zawierać od 2 do 50 znaków.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-zA-Ząćęłńóśźżà-ÿÀ-ß' \-]*$", ErrorMessage = "Nazwa miasta powinna zawierać tylko litery, myślniki, apostrofy oraz zaczynać się wielką literą.")]
        public string? City { get; set; } = string.Empty;

        [OptionalLengthValidator(6, 6, ErrorMessage = "Kod pocztowy powinien zawierać 6 znaków.")]
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Format kodu pocztowego to 'xx-xxx', gdzie x to liczby od 0 do 9.")]
        public string? ZipCode { get; set; } = string.Empty;

        // custom validator
        [OptionalLengthValidator(5, 15, ErrorMessage = "Numer telefonu powinien zawierać od 5 do 15 znaków.")]
        [RegularExpression(@"^(\+?\d{1,4}[\s-]?)?(\(?\d{1,5}\)?[\s-]?)?[\d\s-]{5,15}$", ErrorMessage = "Numer telefonu jest nieprawidłowy.")]
        public string? PhoneNumber { get; set; } = string.Empty;

        [MaxFileSizeValidator(10)]
        public IFormFile? UserPhoto { get; set; }
    }
}
