using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class IngredientTipContentRepository : Repository<IngredientTipContent>, IIngredientTipContentRepository
	{
		public IngredientTipContentRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<IngredientTipContent>> GetIngredientTipContentsWithPaginationAsync(int pageNumber, int pageSize)
		{
			var skipNumber = (pageNumber - 1) * pageSize;
			var query = _dbSet.AsQueryable<IngredientTipContent>();

			var ingredientTipContents = query.Skip(skipNumber).Take(pageSize);
			if (ingredientTipContents.Any())
			{
				return ingredientTipContents;
			}
			return null;
		}

		public async Task UpdateAsync(IngredientTipContent ingredientTipContent)
		{
			var ingredientTipContentFromDb = await this.GetAsync(c => c.Id == ingredientTipContent.Id);

			if (ingredientTipContentFromDb != null)
			{
				if (_context.Entry(ingredientTipContentFromDb).State == EntityState.Detached)
				{
					_context.Attach(ingredientTipContentFromDb);
				}

				ingredientTipContentFromDb.Content = ingredientTipContent.Content;
				ingredientTipContentFromDb.ImageUrl = ingredientTipContent.ImageUrl;

				await _context.SaveChangesAsync();
			}
		}
	}
}
