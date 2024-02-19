using CloudinaryDotNet.Actions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Comment
	{
		[Key]
		public string Id { get; set; } = null!;
		public string UserId { get; set; } = null!;
		public User User { get; set; }
		public string? ParentId { get; set; }
		public Recipe Recipe { get; set; }
		public string Type { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
