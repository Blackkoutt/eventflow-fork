using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.DB.Entities
{
    public class HallType : BaseEntity, INameableEntity, ISoftDeleteable, IPhotoEntity
    {

        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(600)]
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid HallTypeGuid { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string PhotoName { get; set; } = string.Empty;
        public ICollection<Hall> Halls { get; set; } = [];
        public ICollection<Equipment> Equipments { get; set; } = [];

    }
}
