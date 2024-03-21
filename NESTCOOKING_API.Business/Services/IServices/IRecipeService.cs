using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AdminDTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IRecipeService
	{
		Task<IEnumerable<RecipeDTO>> GetAllVerifiedRecipesAsync();
		Task<IEnumerable<RecipeDTO>> GetAllRecipesAsync();
		Task<(int, int, IEnumerable<RecipeDTO>)> GetRecipesAsync(PaginationInfoDTO paginationInfo);
		Task<RecipeDetailDTO> GetRecipeByIdAsync(string id, string? userId);
		Task<IEnumerable<RecipeDTO>> GetRecipesByCategoryIdAsync(int categoryId);
		Task<IEnumerable<RecipeDTO>> GetRecipesByUserIdAsync(string userId);
		Task<RecipeDetailDTO> CreateRecipeAsync(string userId, CreateRecipeDTO recipeDTO);
		Task<RecipeDetailDTO> UpdateRecipeAsync(string userId, string recipeId, UpdateRecipeDTO updateRecipeDTO);
		Task DeleteRecipeAsync(string userId, string id);
		Task<IEnumerable<RecipeDTO>> GetAllFavoriteRecipeAsync(string userId);
		Task SaveFavoriteRecipeAsync(string userId, string recipeId);
		Task RemoveFavoriteRecipeAsync(string userId, string recipeId);
		Task<List<RecipeForBookingDTO>> GetAllRecipesForBookingByChefIdAsync(string chefId);
		Task<RecipeForBookingDTO> UpdateRecipeForBookingAsync(string userId, RecipeForBookingDTO recipeForBookingDTO);

		Task VerifyRecipeAsync(AdminVerifyRecipeDTO adminVerifyRecipeDTO);
	}
}
