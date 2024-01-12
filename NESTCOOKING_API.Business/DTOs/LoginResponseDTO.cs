namespace NESTCOOKING_API.Business.DTOs
{
	public class LoginResponseDTO
	{
		public UserDTO User { get; set; }
		public string Role { get; set; }
		public string Token { get; set; }
	}
}
