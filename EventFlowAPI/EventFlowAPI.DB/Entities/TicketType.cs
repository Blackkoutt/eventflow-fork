using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.DB.Entities
{
    public class TicketType : BaseEntity, INameableEntity, ISoftDeleteable
    {

        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = [];
    }
}
