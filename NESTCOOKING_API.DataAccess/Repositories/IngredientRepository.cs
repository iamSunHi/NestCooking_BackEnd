using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System.Linq;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
	{
		public IngredientRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Ingredient>> GetIngredientsWithPaginationAsync(int pageNumber, int pageSize, string? includeProperties = null)
		{
			var skipNumber = (pageNumber - 1) * pageSize;
			var query = _dbSet.AsQueryable<Ingredient>();

			var ingredients = query.Skip(skipNumber).Take(pageSize);
			if (ingredients.Any())
			{
				if (!string.IsNullOrEmpty(includeProperties))
				{
					foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
					{
						ingredients = ingredients.Include(includeProp.Trim());
					}
				}
				return ingredients;
			}
			return null;
		}

		public async Task UpdateAsync(Ingredient ingredient)
		{
			var ingredientFromDb = await this.GetAsync(i => i.Id == ingredient.Id);

			if (ingredientFromDb != null)
			{
				if (_context.Entry(ingredientFromDb).State == EntityState.Detached)
				{
					_context.Attach(ingredientFromDb);
				}
				ingredientFromDb.Name = ingredient.Name;
				ingredientFromDb.Amount = ingredient.Amount;

				await _context.SaveChangesAsync();
			}
		}
	}
}