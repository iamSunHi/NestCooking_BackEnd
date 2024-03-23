using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class RecipeReaction
    {
        [Key]
        public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public User User { get; set; }
		public string RecipeId { get; set; } = null!;
		public Recipe Recipe { get; set; }
		public string ReactionId { get; set; } = null!;
		public Reaction Reaction { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
