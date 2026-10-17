using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventFlowAPI.Logic.DTO.Abstract
{
    public abstract class StartEndDateAbstract : IDateableRequestDto
    {
        [Required(ErrorMessage = "Data początkowa jest wymagana.")]
        [DataType(DataType.Date)]
        [StartAndEndDateValidator]
        [MaxDurationValidator]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Data końcowa jest wymagana.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [JsonIgnore]
        public virtual TimeSpan? MaxDuration => null;

        [JsonIgnore]
        public virtual string MaxDurationErrorMessage => string.Empty;
    }
}
