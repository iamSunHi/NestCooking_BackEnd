using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class ChefDishRepository : Repository<ChefDish>, IChefDishRepository
	{
		public ChefDishRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task UpdateAsync(ChefDish chefDish)
		{
			var chefDishFromDb = await this.GetAsync(cd => cd.Id == chefDish.Id);

			if (chefDishFromDb != null)
			{
				if (_context.Entry(chefDishFromDb).State == EntityState.Detached)
				{
					_context.Attach(chefDishFromDb);
				}
				chefDishFromDb.Name = chefDish.Name;
				chefDishFromDb.Description = chefDish.Description;
				chefDishFromDb.Price = chefDish.Price;
				chefDishFromDb.Portion = chefDish.Portion;
				chefDishFromDb.RecipeUrl = chefDish.RecipeUrl;
				chefDishFromDb.ImageUrls = chefDish.ImageUrls;

				await _context.SaveChangesAsync();
			}
		}
	}
}
