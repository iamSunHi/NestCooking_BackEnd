namespace NESTCOOKING_API.Business.DTOs
{
	public class RegistrationRequestDTO
	{
		public string UserName { get; set; } = null!;
		public string Password { get; set; } = null!;

		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public bool IsMale { get; set; }
		public string Address { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string PhoneNumber { get; set; } = null!;
	}
}
