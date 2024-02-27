using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class UpdateRecipeDTO
	{
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
		public bool IsPrivate { get; set; }
		public double? Price { get; set; }
		public double Difficult { get; set; }
		public int CookingTime { get; set; }
		public int Portion { get; set; }
		public IEnumerable<int> Categories { get; set; }
		public IEnumerable<CreateIngredientDTO> Ingredients { get; set; }
		public IEnumerable<CreateInstructorDTO> Instructors { get; set; }
	}
}
