using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class IngredientTipShortInfoDTO
	{
		public int Id { get; set; }
		public UserShortInfoDTO User { get; set; }
		public string Title { get; set; } = null!;
	}
}
