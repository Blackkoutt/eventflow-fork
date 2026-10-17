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

        [NotMapped]
        public bool IsExpired => EndDate < DateTime.Now;

        [NotMapped]
        public TimeSpan Duration => EndDate - StartDate;
        public FestivalDetails? Details { get; set; }
        public ICollection<Event> Events { get; set; } = [];
        public ICollection<PartnerBase> Partners { get; set; } = [];

        [NotMapped]
        public IEnumerable<MediaPatron> MediaPatrons => Partners.OfType<MediaPatron>();

        [NotMapped]
        public IEnumerable<Organizer> Organizers => Partners.OfType<Organizer>();

        [NotMapped]
        public IEnumerable<Sponsor> Sponsors => Partners.OfType<Sponsor>();
        public ICollection<Ticket> Tickets { get; set; } = [];
    }
}
