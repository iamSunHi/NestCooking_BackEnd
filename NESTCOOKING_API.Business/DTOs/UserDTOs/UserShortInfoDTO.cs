namespace NESTCOOKING_API.Business.DTOs.UserDTOs
{
	public class UserShortInfoDTO
	{
		public string Id { get; set; } = null!;
		public string UserName { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string? AvatarUrl { get; set; }
	}
}
