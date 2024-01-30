using System.Text.Json.Serialization;

namespace NESTCOOKING_API.Business.DTOs.AuthDTOs
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
    }
}
