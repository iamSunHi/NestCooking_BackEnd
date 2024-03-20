namespace NESTCOOKING_API.Business.DTOs.AdminDTOs
{
	public class AdminUserDTO
	{
		public string Id { get; set; } = null!;
		public string UserName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public bool EmailConfirmed { get; set; }
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string? AvatarUrl { get; set; }
		public string? LockOutEnd { get; set; }
	}
}
