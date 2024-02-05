using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IUserRequestChef
	{
		Task<IEnumerable<RequestToBecomeChef>> GetAllRequests();
		Task<RequestToBecomeChef> GetRequestById(string requestId);
		Task<RequestToBecomeChef> CreateRequestToBecomeChef(RequestToBecomeChef requestToBecomeChef);
		Task<RequestToBecomeChef> UpdateRequestToBecomeChef( RequestToBecomeChef updatedRequest);
		Task<bool> DeleteRequestToBecomeChef(string requestId);


	}
}
