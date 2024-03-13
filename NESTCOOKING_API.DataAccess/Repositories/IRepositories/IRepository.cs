using System.Linq.Expressions;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
		Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
		Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
		Task SaveAsync();
	}
}
