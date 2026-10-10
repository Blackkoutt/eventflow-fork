using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.Identity.DTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.Logic.Identity.DTO.RequestDto
{
    public class UserRegisterRequestDto : IRequestDto
    {
        [Required(ErrorMessage = "Imię jest wymagane.")]
        [Length(2, 40, ErrorMessage = "Imię powinno zawierać od 2 do 40 znaków.")]
        [RegularExpression(@"^[A-ZÀ-Ź][a-zà-ÿąćęłńóśźż]*([ '-][A-ZÀ-Ź]?[a-zà-ÿąćęłńóśźż]*)*$", ErrorMessage = "Imię powinno zawierać tylko litery i zaczynać się wielką literą.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [Length(2, 40, ErrorMessage = "Nazwisko powinno zawierać od 2 do 40 znaków.")]
        [RegularExpression(@"^[A-ZÀ-Ź][a-zà-ÿąćęłńóśźż]*([ '-][A-ZÀ-Ź]?[a-zà-ÿąćęłńóśźż]*)*$", ErrorMessage = "Nazwisko powinno zawierać tylko litery i zaczynać się wielką literą.")]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [Length(5, 255, ErrorMessage = "Adres e-mail powinien zawierać od 5 do 255 znaków.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Adres e-mail ma niepoprawny format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data urodzenia jest wymagana.")]
        [DataType(DataType.Date)]
        [DateOfBirthValidator(13)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Hasła powinny być takie same.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
