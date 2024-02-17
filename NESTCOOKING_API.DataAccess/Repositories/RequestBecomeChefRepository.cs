using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class RequestBecomeChefRepository : Repository<RequestToBecomeChef>, IRequestBecomeChefRepository
	{
		private readonly ApplicationDbContext _context;

		public RequestBecomeChefRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;

		}
		public async Task<RequestToBecomeChef> CreateRequestToBecomeChef(RequestToBecomeChef requestToBecomeChef)
		{
			_context.RequestToBecomeChefs.Add(requestToBecomeChef);
			await _context.SaveChangesAsync();
			return requestToBecomeChef;
		}
		public async Task<bool> DeleteRequestToBecomeChef(string requestId)
		{
			var requestToDelete = await GetRequestById(requestId);

			if (requestToDelete != null)
			{
				_context.RequestToBecomeChefs.Remove(requestToDelete);
				await _context.SaveChangesAsync();
				return true;
			}

			return false;
		}
		public async Task<IEnumerable<RequestToBecomeChef>> GetAllAsync()
		{
			var requets = await _context.RequestToBecomeChefs.ToListAsync();
			return requets;
		}
		public async Task<RequestToBecomeChef> GetRequestById(string requestId)
		{
			var requestBecomeChef = await _context.RequestToBecomeChefs.FindAsync(requestId);
			return requestBecomeChef;
		}
		public async Task<RequestToBecomeChef> UpdateRequestToBecomeChef(RequestToBecomeChef updatedRequest)
		{
			_context.Update(updatedRequest);
			await _context.SaveChangesAsync();
			return updatedRequest;
		}
	}
}
