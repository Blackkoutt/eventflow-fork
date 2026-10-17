using EventFlowAPI.Logic.DTO.Abstract;
using EventFlowAPI.Logic.DTO.RequestDto;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.Logic.DTO.Validators
{
    public sealed class MaxDurationValidator : ValidationAttribute
    {
        protected sealed override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is StartEndDateAbstract datesObj)
            {
                TimeSpan duration = datesObj.EndDate - datesObj.StartDate;

                if (datesObj.MaxDuration != null && duration > datesObj.MaxDuration)
                {
                    return new ValidationResult(datesObj.MaxDurationErrorMessage);
                }
            }
            if (validationContext.ObjectInstance is FestivalUpdate_EventRequestDto requestDto)
            {
                if(requestDto.EndDate != null && requestDto.StartDate != null)
                {
                    TimeSpan duration = (DateTime)requestDto.EndDate - (DateTime)requestDto.StartDate;

                    if (duration > requestDto.MaxDuration)
                    {
                        return new ValidationResult(requestDto.MaxDurationErrorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
