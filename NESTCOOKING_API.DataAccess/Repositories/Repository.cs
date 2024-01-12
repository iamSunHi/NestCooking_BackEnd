using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using System.Linq.Expressions;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		internal DbSet<T> _dbSet;

		public Repository(ApplicationDbContext context)
		{
			_context = context;
			this._dbSet = _context.Set<T>();
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
		{
			IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}
			return await query.ToListAsync();
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
		{
			IQueryable<T> query = _dbSet;

			return await query.FirstOrDefaultAsync(filter);
		}

		public async Task CreateAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await SaveAsync();
		}

		public async Task RemoveAsync(T entity)
		{
			_dbSet.Remove(entity);
			await SaveAsync();
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
