using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class CommentReaction
	{
		[Key]
		public int Id { get; set; }
		public Comment Comment { get; set; }
		public Reaction Reaction { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
