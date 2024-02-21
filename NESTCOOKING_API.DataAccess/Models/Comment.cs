using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Comment
	{
		[Key]
		public string CommentId { get; set; } = null!;
		public string UserId { get; set; } = null!;
		public User User { get; set; }
		public string RecipeId { get; set; } = null!;
		public Recipe Recipe { get; set; }
		public string Type { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string? ParentCommentId { get; set; }
		public ICollection<Comment> ChildComments { get; set; }
	}
}
