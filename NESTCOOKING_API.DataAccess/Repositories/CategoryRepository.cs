using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		public CategoryRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Category>> GetCategoriesWithPaginationAsync(int pageNumber, int pageSize)
		{
			var skipNumber = (pageNumber - 1) * pageSize;
			var query = _dbSet.AsQueryable<Category>();

			var categories = query.Skip(skipNumber).Take(pageSize);
			if (categories.Any())
			{
				return categories;
			}
			return null;
		}

		public async Task UpdateAsync(Category category)
		{
			var categoryFromDb = await this.GetAsync(c => c.Id == category.Id);

			if (categoryFromDb != null)
			{
				if (_context.Entry(categoryFromDb).State == EntityState.Detached)
				{
					_context.Attach(categoryFromDb);
				}

				categoryFromDb.Name = category.Name;
				await _context.SaveChangesAsync();
			}
		}
	}
}
