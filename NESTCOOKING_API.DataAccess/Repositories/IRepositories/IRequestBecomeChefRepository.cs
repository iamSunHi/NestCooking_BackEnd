using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IRequestBecomeChefRepository : IRepository<RequestToBecomeChef>
	{
		Task<RequestToBecomeChef> CreateRequestToBecomeChef(RequestToBecomeChef requestToBecomeChef);
		Task<RequestToBecomeChef> UpdateRequestToBecomeChef(RequestToBecomeChef updatedRequest);
		
	}
}
