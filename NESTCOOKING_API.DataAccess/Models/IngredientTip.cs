using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class IngredientTip
	{
		[Key]
		public int Id { get; set; }
		public User User { get; set; }
		public IEnumerable<IngredientTipContent> Contents { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public IEnumerable<Ingredient>? Ingredients { get; set; }
	}
}
