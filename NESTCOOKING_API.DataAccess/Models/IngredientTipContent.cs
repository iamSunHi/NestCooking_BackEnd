using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class IngredientTipContent
	{
		[Key]
		public int Id { get; set; }
		public string IngredientTipId { get; set; } = null!;
		public string Content { get; set; } = null!;
		public string? ImageUrl { get; set; }
	}
}
