using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class IngredientTipDTO
	{
		public int? Id { get; set; }
		public UserShortInfoDTO User { get; set; }
		public IEnumerable<IngredientTipContentDTO> Contents { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
