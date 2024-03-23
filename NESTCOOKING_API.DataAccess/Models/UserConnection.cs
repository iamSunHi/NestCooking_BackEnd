namespace NESTCOOKING_API.DataAccess.Models
{
	public class UserConnection
	{
		public string UserId { get; set; } = null!;
		public string FollowingUserId { get; set; } = null!;
	}
}