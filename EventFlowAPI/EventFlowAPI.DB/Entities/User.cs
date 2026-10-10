using EventFlowAPI.DB.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.DB.Entities
{
    public class User : IEntity, IPhotoEntity, IUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Role { get; set; } = string.Empty;

        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(40)]
        public string Surname { get; set; } = string.Empty;
        public DateTime RegisteredDate { get; set; }
        public string PhotoName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public UserData? UserData { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = [];
        public ICollection<EventPass> EventPasses { get; set; } = [];
        public ICollection<HallRent> HallRents { get; set; } = [];
    }
}
