using System.Text.Json.Serialization;

namespace NESTCOOKING_API.Business.DTOs.UserDTOs
{
    public class UserDetailInfoDTO
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool? IsMale { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public double? BookingPrice { get; set; }
        public double? Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Role { get; set; } = null!;
    }
}
