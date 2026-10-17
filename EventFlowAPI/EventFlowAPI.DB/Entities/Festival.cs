using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFlowAPI.DB.Entities
{
    public class Festival : BaseEntity, ISoftDeleteable, IExpireable, IDateableEntity, INameableEntity, IPhotoEntity
    {

        [MaxLength(60)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string ShortDescription { get; set; } = string.Empty;
        public DateTime AddDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid FestivalGuid { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }
        public string PhotoName { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? LongDescription { get; set; }

        [NotMapped]
        public bool IsExpired => EndDate < DateTime.Now;

        [NotMapped]
        public TimeSpan Duration => EndDate - StartDate;
        public ICollection<Event> Events { get; set; } = [];
        public ICollection<Partner> Partners { get; set; } = [];
        public ICollection<Ticket> Tickets { get; set; } = [];
    }
}
