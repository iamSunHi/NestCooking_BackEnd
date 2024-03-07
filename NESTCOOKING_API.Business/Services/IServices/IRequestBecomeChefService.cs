using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IRequestBecomeChefService
	{
		Task<IEnumerable<RequestToBecomeChefDTO>> GetAllRequestsToBecomeChef();
		Task<RequestToBecomeChefDTO> GetRequestToBecomeChefById(string requestId);
		Task<RequestToBecomeChefDTO> CreateRequestToBecomeChef(string userId, CreatedRequestToBecomeChefDTO requestToBecomeChefDTO);
		Task<RequestToBecomeChefDTO> UpdateRequestToBecomeChef(string requestId, CreatedRequestToBecomeChefDTO requestToBecomeChefDTO);
		Task DeleteRequestToBecomeChef(string requestId);
		Task<RequestToBecomeChefDTO> GetRequestToBecomeChefByUserId(string userId);
        Task<RequestToBecomeChefDTO> ApprovalRequestByAdmin(string requestId,ApprovalRequestDTO approvalRequestDTO);


    }
}
