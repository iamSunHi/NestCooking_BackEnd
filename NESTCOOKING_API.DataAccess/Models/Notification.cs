using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Notification
	{
		[Key]
		public string Id { get; set; } = null!;
		public User User { get; set; }
		public User RelatedUser { get; set; }
		public string NotificationType { get; set; } = null!;
		public string Content { get; set; } = null!;
		public bool IsSeen { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
