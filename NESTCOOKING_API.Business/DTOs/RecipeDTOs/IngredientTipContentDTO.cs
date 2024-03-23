namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class IngredientTipContentDTO
	{
		public int? Id { get; set; }
		public string Content { get; set; } = null!;
		public string? ImageUrl { get; set; }
	}
}
