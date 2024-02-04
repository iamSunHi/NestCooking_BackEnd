using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class RecipeDetailDTO
	{
		public string Id { get; set; } = null!;
		public UserShortInfoDTO Owner { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
		public double Ratings { get; set; }
		public int CookingTime { get; set; }
		public int Portion { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Ingredient> Ingredients { get; set; }
		public IEnumerable<Instructor> Instructors { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
