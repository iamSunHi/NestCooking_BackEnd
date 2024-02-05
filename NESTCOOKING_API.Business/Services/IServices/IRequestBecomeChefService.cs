using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IRequestBecomeChefService
	{
		Task<IEnumerable<RequestToBecomeChef>> GetAllRequestsToBecomChef();
		Task<RequestToBecomeChef> GetRequestToBecomeChefById(string requestId);
		Task<RequestToBecomeChef> CreateRequestToBecomeChef(string userId, RequestToBecomeChefDTO requestToBecomeChefDTO);
		Task<RequestToBecomeChefDTO> UpdateRequestToBecomeChef(string requestId, RequestToBecomeChefDTO requestToBecomeChefDTO);
		Task<bool> DeleteRequestToBecomeChef(string requestId);
	}
}
