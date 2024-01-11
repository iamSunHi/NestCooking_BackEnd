using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Comment
	{
		[Key]
		public string Id { get; set; } = null!;
		public User User { get; set; }
		public Post Post { get; set; }
		public string Content { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
