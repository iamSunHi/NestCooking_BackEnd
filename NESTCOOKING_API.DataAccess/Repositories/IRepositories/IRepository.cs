using System.Linq.Expressions;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
		Task<T> GetAsync(Expression<Func<T, bool>> filter);
		Task CreateAsync(T entity);
		Task RemoveAsync(T entity);
		Task SaveAsync();
	}
}
