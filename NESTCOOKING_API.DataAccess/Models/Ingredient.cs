using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Ingredient
	{
		[Key]
		public int Id { get; set; }
		public string RecipeId { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string Amount { get; set; } = null!;
		public string? IngredientTipId { get; set; }
	}
}
