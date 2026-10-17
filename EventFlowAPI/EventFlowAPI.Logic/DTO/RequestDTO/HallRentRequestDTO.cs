using EventFlowAPI.Logic.DTO.Abstract;
using EventFlowAPI.Logic.DTO.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventFlowAPI.Logic.DTO.RequestDto
{
    public class HallRentRequestDto : StartEndDateAbstract, IRequestDto, ICollisionalRequestDto
    {

        [Required(ErrorMessage = "Należy wybrać salę.")]
        public int HallId { get; set; }        

        public List<int> AdditionalServicesIds { get; set; } = [];

    }
}
