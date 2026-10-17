using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.Validators;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.Logic.DTO.RequestDto
{
    public class PartnerRequestDto : IRequestDto, INameableRequestDto
    {
        [Required(ErrorMessage = "Rodzaj partnera jest wymagany.")]
        public PartnerKind? Kind { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [Length(2, 50, ErrorMessage = "Nazwa powinna zawierać od 2 do 50 znaków.")]
        public string Name { get; set; } = string.Empty;

        [MaxFileSizeValidator(10)]
        public IFormFile? PartnerPhoto { get; set; }
    }
}
