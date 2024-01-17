using System.Text.Json.Serialization;

namespace NESTCOOKING_API.Business.DTOs
{
	public class LoginResponseDTO
	{
		[JsonPropertyName("access_token")]
		public string AccessToken { get; set; }
    }
}
