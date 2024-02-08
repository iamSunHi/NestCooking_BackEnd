using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IIngredientTipService
	{
		Task<IEnumerable<IngredientTipDTO>> GetAllIngredientTipsAsync();
		Task<IEnumerable<IngredientTipDTO>> GetIngredientTipsAsync(PaginationInfoDTO paginationInfo);
		Task<IngredientTipDTO> GetIngredientTipByIdAsync(int id);
		Task<IngredientTipShortInfoDTO> GetIngredientTipShortInfoByIdAsync(int id);
		Task CreateIngredientTipAsync(IngredientTipDTO ingredientTipDTO);
		Task UpdateIngredientTipAsync(IngredientTipDTO ingredientTipDTO);
		Task DeleteIngredientTipAsync(int id);
	}
}
