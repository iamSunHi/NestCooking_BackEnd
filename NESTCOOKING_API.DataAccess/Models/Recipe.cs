using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Recipe
	{
		[Key]
		public string Id { get; set; } = null!;
		public int CookingTime { get; set; }
		public int Portion { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Ingredient> Ingredients { get; set; }
		public IEnumerable<Instructor> Instructors { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
