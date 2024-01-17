using System.Text.Json.Serialization;

namespace NESTCOOKING_API.Business.DTOs
{
	public class UserInfoDTO
	{
		[JsonPropertyName("first_name")]
		public string FirstName { get; set; }
		[JsonPropertyName("last_name")]
		public string LastName { get; set; }
		[JsonPropertyName("is_male")]
		public bool IsMale {  get; set; }
		[JsonPropertyName("phone_number")]
		public string PhoneNumber { get; set; }
		[JsonPropertyName("email")]
		public string Email { get; set; }
		[JsonPropertyName("address")]
		public string Address { get; set; }
		[JsonPropertyName("avatar_url")]
		public string AvatarUrl { get; set; }
	}
}
