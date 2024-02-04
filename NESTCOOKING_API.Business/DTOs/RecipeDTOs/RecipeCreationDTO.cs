using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class RecipeCreationDTO
	{
		public string UserId { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
		public bool IsPrivate { get; set; }
		public double? Price { get; set; }
		public int Portion { get; set; }
		public int CookingTime { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Ingredient> Ingredients { get; set; }
		public IEnumerable<Instructor> Instructors { get; set; }
	}
}
