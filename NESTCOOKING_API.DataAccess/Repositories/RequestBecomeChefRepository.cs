using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class RequestBecomeChefRepository : Repository<RequestToBecomeChef>, IRequestBecomeChefRepository
	{
		public RequestBecomeChefRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<RequestToBecomeChef> CreateRequestToBecomeChef(RequestToBecomeChef requestToBecomeChef)
		{
			_context.RequestToBecomeChefs.Add(requestToBecomeChef);
			await _context.SaveChangesAsync();
			return requestToBecomeChef;
		}

		public async Task<RequestToBecomeChef> UpdateRequestToBecomeChef(RequestToBecomeChef updatedRequest)
		{
			_context.Update(updatedRequest);
			await _context.SaveChangesAsync();
			return updatedRequest;
		}
	}
}
