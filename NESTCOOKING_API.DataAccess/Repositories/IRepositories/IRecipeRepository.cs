using NESTCOOKING_API.DataAccess.Models;
using System.Linq.Expressions;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IRecipeRepository : IRepository<Recipe>
	{
		Task<IEnumerable<Recipe>> GetRecipesWithPaginationAsync(int pageNumber, int pageSize, string? includeProperties = null);
		Task<Recipe> GetRecipeByIdAsync(string recipeId);
		Task<IEnumerable<Recipe>> GetRecipesByCategoryIdAsync(int categoryId);
		Task UpdateAsync(Recipe recipe);
	}
}
