using System.Text.Json.Serialization;

namespace NESTCOOKING_API.Business.DTOs
{
	public class UserInfoDTO
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsMale {  get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public string AvatarUrl { get; set; }
		public double? BookingPrice { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string Role { get; set; }
	}
}
