using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class CommentReaction
	{
		[Key]
		public int Id { get; set; }
 		public User User { get; set; } = null!;
		public Comment Comment { get; set; } = null!;
		public Reaction Reaction { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
