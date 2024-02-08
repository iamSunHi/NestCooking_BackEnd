namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class IngredientDTO
	{
		public int? Id { get; set; }
		public string Name { get; set; } = null!;
		public string Amount { get; set; } = null!;
		public IngredientTipShortInfoDTO? IngredientTip { get; set; }
	}
}
