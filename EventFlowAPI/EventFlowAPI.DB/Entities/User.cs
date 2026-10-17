using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.DB.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventFlowAPI.DB.Entities
{
    public class User : IEntity, IPhotoEntity, IUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public Roles Role { get; set; }

        [MaxLength(40)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(40)]
        public string Surname { get; set; } = string.Empty;
        public DateTime RegisteredDate { get; set; }
        public string PhotoName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        [MaxLength(50)]
        public string? Street { get; set; } = string.Empty;

        [Range(0, 999),
         Column(TypeName = "NUMERIC(3)")]
        public int? HouseNumber { get; set; }

        [Range(0, 999),
         Column(TypeName = "NUMERIC(3)")]
        public int? FlatNumber { get; set; }

        [MaxLength(50)]
        public string? City { get; set; } = string.Empty;

        [MaxLength(6)]
        public string? ZipCode { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? PhoneNumber { get; set; } = string.Empty;

        [NotMapped]
        public bool HasUserData => !string.IsNullOrEmpty(Street) || HouseNumber != null || FlatNumber != null ||
                                   !string.IsNullOrEmpty(City) || !string.IsNullOrEmpty(ZipCode) || !string.IsNullOrEmpty(PhoneNumber);

        public ICollection<Reservation> Reservations { get; set; } = [];
        public ICollection<EventPass> EventPasses { get; set; } = [];
        public ICollection<HallRent> HallRents { get; set; } = [];
    }
}
