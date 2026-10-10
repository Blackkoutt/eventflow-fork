using EventFlowAPI.DB.Entities.Abstract;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EventFlowAPI.DB.Entities
{
    public class User : IdentityUser, IEntity, IPhotoEntity, IUser
    {

        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(40)]
        public string Surname { get; set; } = string.Empty;
        public DateTime RegisteredDate { get; set; }
        public bool IsVerified { get; set; }
        public string PhotoName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public UserData? UserData { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = [];
        public ICollection<EventPass> EventPasses { get; set; } = [];
        public ICollection<HallRent> HallRents { get; set; } = [];

        public ICollection<Role> Roles { get; set; } = [];
    }
}
