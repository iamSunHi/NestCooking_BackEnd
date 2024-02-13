using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Instructor
	{
		[Key]
		public int Id { get; set; }
		public string RecipeId { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ImageUrl { get; set; }
		public int StepNumber { get; set; }
	}
}
