using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class CategoryRecipeRepository : Repository<CategoryRecipe>, ICategoryRecipeRepository
	{
		private readonly IRecipeRepository _recipeRepository;
		private readonly ICategoryRepository _categoryRepository;

		public CategoryRecipeRepository(ApplicationDbContext context,
			IRecipeRepository recipeRepository, ICategoryRepository categoryRepository) : base(context)
		{
			_recipeRepository = recipeRepository;
			_categoryRepository = categoryRepository;
		}

		public async Task<IEnumerable<Recipe>> GetRecipesByCategoryIdAsync(int categoryId)
		{
			var categoryRecipeList = await this.GetAllAsync(rc => rc.CategoryId == categoryId);

			var recipeList = new List<Recipe>();
			foreach (var categoryRecipe in categoryRecipeList)
			{
				var recipe = await _recipeRepository.GetAsync(r => r.Id == categoryRecipe.RecipeId);
				recipeList.Add(recipe);
			}

			return recipeList;
		}

		public async Task<IEnumerable<Category>> GetCategoriesByRecipeIdAsync(string recipeId)
		{
			var categoryRecipeList = await this.GetAllAsync(rc => rc.RecipeId == recipeId);

			var categoryList = new List<Category>();
			foreach (var categoryRecipe in categoryRecipeList)
			{
				var category = await _categoryRepository.GetAsync(c => c.Id == categoryRecipe.CategoryId);
				categoryList.Add(category);
			}

			return categoryList;
		}
	}
}
