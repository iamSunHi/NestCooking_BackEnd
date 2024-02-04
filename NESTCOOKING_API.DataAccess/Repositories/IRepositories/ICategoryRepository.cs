using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface ICategoryRepository : IRepository<Category>
	{
		Task<IEnumerable<Category>> GetCategoriesWithPaginationAsync(int pageNumber, int pageSize);
		Task UpdateAsync(Category category);
	}
}
