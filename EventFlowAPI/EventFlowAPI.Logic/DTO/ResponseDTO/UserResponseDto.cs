using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.DTO.Interfaces;

namespace EventFlowAPI.Logic.DTO.ResponseDto
{
    public class UserResponseDto : IUser, IResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
       // public string Email { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime RegisteredDate { get; set; }
        public int AllReservationsCount { get; set; }
        public int AllHallRentsCount { get; set; }
        public bool IsActiveEventPass { get; set; }
        public string PhotoName { get; set; } = string.Empty;
        public string PhotoEndpoint { get; set; } = string.Empty;
        public string? Street { get; set; }
        public int? HouseNumber { get; set; }
        public int? FlatNumber { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
        public IList<string?> UserRoles { get; set; } = [];
    }
}
