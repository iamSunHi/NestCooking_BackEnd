using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IRecipeRepository : IRepository<Recipe>
	{
		Task<IEnumerable<Recipe>> GetRecipesWithPaginationAsync(int pageNumber, int pageSize);
		Task UpdateAsync(Recipe recipe);
		Task<IEnumerable<Recipe>> GetRecipesByCriteriaAsync(string criteria, string? userId = null);
		Task UpdateRecipeForBookingAsync(Recipe recipe);
	}
}
