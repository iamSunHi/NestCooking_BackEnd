using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class RecipeRepository : Repository<Recipe>, IRecipeRepository
	{
		private readonly ICategoryRepository _categoryRepository;

		public RecipeRepository(ApplicationDbContext context, ICategoryRepository categoryRepository) : base(context)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<IEnumerable<Recipe>> GetRecipesWithPaginationAsync(int pageNumber, int pageSize, string? includeProperties = null)
		{
			var skipNumber = (pageNumber - 1) * pageSize;
			var query = _dbSet.AsQueryable<Recipe>();

			var recipes = query.Skip(skipNumber).Take(pageSize);
			if (recipes.Any())
			{
				if (!string.IsNullOrEmpty(includeProperties))
				{
					foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
					{
						recipes = recipes.Include(includeProp.Trim());
					}
				}
				return recipes;
			}
			return null;
		}

		public async Task<Recipe> GetRecipeByIdAsync(string recipeId)
		{
			var recipe = await _context.Recipes
				.Where(r => r.Id == recipeId)
				.Include(r => r.User)
				.Include(r => r.Categories)
				.Include(r => r.Ingredients).ThenInclude(i => i.IngredientTip)
				.FirstOrDefaultAsync();
			if (recipe == null)
			{
				return null;
			}
			else
			{
				return recipe;
			}
		}

		public async Task<IEnumerable<Recipe>> GetRecipesByCategoryIdAsync(int categoryId)
		{
			var categories = await _categoryRepository.GetAsync(c => c.Id == categoryId, includeProperties: "Recipes");

			var recipeList = categories.Recipes.ToList();
			foreach (var recipe in recipeList)
			{
				await _context.Entry(recipe)
					.Reference(r => r.User)
					.LoadAsync();
			}

			return recipeList;
		}


		public async Task UpdateAsync(Recipe recipe)
		{
			var recipeFromDb = await this.GetAsync(c => c.Id == recipe.Id);

			if (recipeFromDb != null)
			{
				if (_context.Entry(recipeFromDb).State == EntityState.Detached)
				{
					_context.Attach(recipeFromDb);
				}

				await _context.SaveChangesAsync();
			}
		}

	}
}
