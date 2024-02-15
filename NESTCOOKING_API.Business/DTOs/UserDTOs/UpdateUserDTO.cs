using System.Text.Json.Serialization;

namespace NESTCOOKING_API.Business.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsMale { get; set; }
        public string Address { get; set; }
        public string AvatarUrl { get; set; }
    }
}
