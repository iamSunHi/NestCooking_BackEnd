using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Recipe
	{
		[Key]
		public string Id { get; set; } = null!;
		public string UserId { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
		public bool IsPrivate { get; set; }
		public double? Price { get; set; }
		public double Ratings { get; set; }
		public int CookingTime { get; set; }
		public int Portion { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		// public IEnumerable<Reaction> Reactions { get; set; }
	}
}
