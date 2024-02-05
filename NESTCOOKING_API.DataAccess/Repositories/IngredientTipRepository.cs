using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System.Linq;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class IngredientTipRepository : Repository<IngredientTip>, IIngredientTipRepository
	{
		public IngredientTipRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<IngredientTip>> GetIngredientTipsWithPaginationAsync(int pageNumber, int pageSize, string? includeProperties = null)
		{
			var skipNumber = (pageNumber - 1) * pageSize;
			var query = _dbSet.AsQueryable<IngredientTip>();

			var ingredientTips = query.Skip(skipNumber).Take(pageSize);
			if (ingredientTips.Any())
			{
				if (!string.IsNullOrEmpty(includeProperties))
				{
					foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
					{
						ingredientTips = ingredientTips.Include(includeProp.Trim());
					}
				}
				return ingredientTips;
			}
			return null;
		}

		public async Task UpdateAsync(IngredientTip ingredientTip)
		{
			var ingredientTipFromDb = await this.GetAsync(i => i.Id == ingredientTip.Id);

			if (ingredientTipFromDb != null)
			{
				if (_context.Entry(ingredientTipFromDb).State == EntityState.Detached)
				{
					_context.Attach(ingredientTipFromDb);
				}
				ingredientTipFromDb.UpdatedAt = DateTime.UtcNow;

				await _context.SaveChangesAsync();
			}
		}
	}
}
