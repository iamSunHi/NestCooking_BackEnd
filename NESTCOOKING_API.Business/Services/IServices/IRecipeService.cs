using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IRecipeService
	{
		Task<IEnumerable<RecipeDTO>> GetAllRecipesAsync();
		Task<IEnumerable<RecipeDTO>> GetRecipesAsync(PaginationInfoDTO paginationInfo);
		Task<RecipeDetailDTO> GetRecipeByIdAsync(string id);
		Task<IEnumerable<RecipeDTO>> GetRecipesByCategoryIdAsync(int categoryId);
		Task CreateRecipeAsync(RecipeDetailDTO recipeDTO);
		Task UpdateRecipeAsync(RecipeDetailDTO recipeDetailDTO);
		Task DeleteRecipeAsync(string id);
	}
}
