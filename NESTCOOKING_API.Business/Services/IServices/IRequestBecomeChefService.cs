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
		Task<IEnumerable<RequestToBecomeChefDTO>> GetAllRequestsToBecomeChef();
		Task<RequestToBecomeChefDTO> GetRequestToBecomeChefById(string requestId);
		Task<RequestToBecomeChefDTO> CreateRequestToBecomeChef(string userId, CreatedRequestToBecomeChefDTO requestToBecomeChefDTO);
		Task<RequestToBecomeChefDTO> UpdateRequestToBecomeChef(string requestId, CreatedRequestToBecomeChefDTO requestToBecomeChefDTO);
		Task<bool> DeleteRequestToBecomeChef(string requestId);
	}
}
