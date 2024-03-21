namespace NESTCOOKING_API.Business.DTOs.AdminDTOs
{
	public class AdminUserDTO
	{
		public string Id { get; set; } = null!;
		public string UserName { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Gender { get; set; } = null!;
		public string Role { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string? PhoneNumber { get; set; }
		public string? Address { get; set; }
		public DateTime? LockOutEndTime { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
