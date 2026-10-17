using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFlowAPI.DB.Entities
{
    public class Event : BaseEntity, ISoftDeleteable, IExpireable, IDateableEntity, INameableEntity, IPhotoEntity
    {

        [MaxLength(60)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string ShortDescription { get; set; } = string.Empty;

        public DateTime AddDate { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public int CategoryId { get; set; } 
        public int HallId { get; set; }
        public EventCategory Category { get; set; } = default!;
        public Hall Hall { get; set; } = default!;
        public ICollection<Festival> Festivals { get; set; } = [];
        public ICollection<Ticket> Tickets { get; set; } = [];

        [NotMapped]
        public bool IsExpired => EndDate < DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }
        public Guid EventGuid { get; set; }
        public string PhotoName { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? LongDescription { get; set; }

        [NotMapped]
        public TimeSpan Duration => EndDate - StartDate;
    }
}
