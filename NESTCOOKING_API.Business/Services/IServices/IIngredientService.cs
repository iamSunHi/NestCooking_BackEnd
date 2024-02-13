using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IIngredientService
	{
		Task<IEnumerable<IngredientDTO>> GetAllIngredientsAsync();
		Task<IEnumerable<IngredientDTO>> GetIngredientsAsync(PaginationInfoDTO paginationInfo);
		Task<IngredientDTO> GetIngredientByIdAsync(int id);
		Task CreateIngredientAsync(IngredientDTO ingredientDTO);
		Task UpdateIngredientAsync(IngredientDTO ingredientDTO);
		Task DeleteIngredientAsync(int id);
	}
}
