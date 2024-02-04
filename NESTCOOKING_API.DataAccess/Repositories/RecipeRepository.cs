using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class RecipeRepository : Repository<Recipe>, IRecipeRepository
	{
		private readonly ApplicationDbContext _context;

		public RecipeRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Recipe>> GetRecipesWithPaginationAsync(int pageNumber, int pageSize)
		{
			var skipNumber = (pageNumber - 1) * pageSize;
			var query = _dbSet.AsQueryable<Recipe>();

			var recipes = query.Skip(skipNumber).Take(pageSize);
			if (recipes.Any())
			{
				return recipes;
			}
			return null;
		}
	}
}
