using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface ICategoryRecipeRepository : IRepository<CategoryRecipe>
	{
		Task<IEnumerable<Recipe>> GetRecipesByCategoryIdAsync(int categoryId);
		Task<IEnumerable<Category>> GetCategoriesByRecipeIdAsync(string recipeId);
	}
}
