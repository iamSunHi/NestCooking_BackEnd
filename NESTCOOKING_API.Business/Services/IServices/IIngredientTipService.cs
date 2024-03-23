using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IIngredientTipService
	{
		Task<IEnumerable<IngredientTipShortInfoDTO>> GetAllIngredientTipsAsync();
		Task<(int, int, IEnumerable<IngredientTipShortInfoDTO>)> GetIngredientTipsAsync(PaginationInfoDTO paginationInfo);
		Task<IngredientTipDTO> GetIngredientTipByIdAsync(string id);
		Task<IngredientTipShortInfoDTO> GetIngredientTipShortInfoByIdAsync(string id);
		Task CreateIngredientTipAsync(string userId, IngredientTipDTO ingredientTipDTO);
		Task UpdateIngredientTipAsync(string userId, IngredientTipDTO ingredientTipDTO);
		Task DeleteIngredientTipAsync(string userId, string id);
	}
}
