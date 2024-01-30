using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Recipe
	{
		[Key]
		public string Id { get; set; } = null!;
		public User User { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
		public bool IsPrivate { get; set; }
		public double? Price { get; set; }
		public double Ratings { get; set; }
		public int CookingTime { get; set; }
		public int Portion { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Ingredient> Ingredients { get; set; }
		public IEnumerable<Instructor> Instructors { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public IEnumerable<Reaction> Reactions { get; set; }
	}
}
