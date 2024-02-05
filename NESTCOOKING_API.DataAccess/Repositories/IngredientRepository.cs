using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
	{
		public IngredientRepository(ApplicationDbContext context) : base(context)
		{
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