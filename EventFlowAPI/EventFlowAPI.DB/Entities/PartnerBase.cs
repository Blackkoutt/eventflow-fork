using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.DB.Entities
{
    public abstract class PartnerBase : BaseEntity, INameableEntity, ISoftDeleteable, IPhotoEntity
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }
        public Guid PartnerGuid { get; set; }
        public string PhotoName { get; set; } = string.Empty;

        public ICollection<Festival> Festivals { get; set; } = [];
    }
}
