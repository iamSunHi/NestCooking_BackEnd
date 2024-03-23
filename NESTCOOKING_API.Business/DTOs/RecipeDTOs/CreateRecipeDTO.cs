using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class CreateRecipeDTO
	{
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
		public double Difficult { get; set; }
		public int CookingTime { get; set; }
		public int Portion { get; set; }
		public IEnumerable<int> Categories { get; set; }
		public IEnumerable<CreateIngredientDTO> Ingredients { get; set; }
		public IEnumerable<CreateInstructorDTO> Instructors { get; set; }

		[DefaultValue(false)]
		public bool IsPrivate { get; set; }
		[Range(0.0, double.MaxValue)]
		public double? RecipePrice { get; set; }
		[DefaultValue(false)]
		public bool IsAvailableForBooking { get; set; }
		[Range(0.0, double.MaxValue)]
		public double? BookingPrice { get; set; }
	}
}
