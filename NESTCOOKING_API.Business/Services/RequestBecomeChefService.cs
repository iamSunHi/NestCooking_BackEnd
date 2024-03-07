using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Business.Services
{
    public class RequestBecomeChefService : IRequestBecomeChefService
    {
        private readonly IRequestBecomeChefRepository _chefRequestRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RequestBecomeChefService(IRequestBecomeChefRepository chefRequestRepository, UserManager<User> userManager, IMapper mapper)
        {
            _chefRequestRepository = chefRequestRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<RequestToBecomeChefDTO> CreateRequestToBecomeChef(string userId, CreatedRequestToBecomeChefDTO requestToBecomeChefDTO)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    throw new UserNotFoundException();
                }

                var existedRequest = await this.GetRequestToBecomeChefByUserId(userId);

                if (existedRequest != null && existedRequest.Status.Equals(ActionStatus_ACCEPTED))
                {
                    throw new Exception(AppString.RequestExistedErrorMessage);
                }

                if (existedRequest != null && existedRequest.Status.Equals(ActionStatus_PENDING))
                {
                    throw new Exception(AppString.RequestAlreadyHandledErrorMessage);
                }

                var requestToBecomeChef = _mapper.Map<RequestToBecomeChef>(requestToBecomeChefDTO);
                requestToBecomeChef.RequestChefId = Guid.NewGuid().ToString();
                requestToBecomeChef.UserID = userId;
                requestToBecomeChef.Status = ActionStatus_PENDING;
                requestToBecomeChef.ResponseId = null;
                requestToBecomeChef.CreatedAt = DateTime.UtcNow;

                var result = this._mapper.Map<RequestToBecomeChefDTO>(await _chefRequestRepository.CreateRequestToBecomeChef(requestToBecomeChef));

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RequestToBecomeChefDTO> UpdateRequestToBecomeChef(string requestId, CreatedRequestToBecomeChefDTO requestToBecomeChefDTO)
        {
            var existingRequest = await _chefRequestRepository.GetAsync(request => request.RequestChefId == requestId);

            if (existingRequest != null)
            {
                existingRequest.CreatedAt = DateTime.UtcNow;
                _mapper.Map(requestToBecomeChefDTO, existingRequest);
                await _chefRequestRepository.UpdateRequestToBecomeChef(existingRequest);
                var updatedDto = _mapper.Map<RequestToBecomeChefDTO>(existingRequest);
                return updatedDto;
            }
            return null;
        }

        public async Task DeleteRequestToBecomeChef(string requestId)
        {
            var requestFromDb = await _chefRequestRepository.GetAsync(request => request.RequestChefId == requestId);
            if (requestFromDb == null)
            {
                throw new Exception(AppString.RequestBecomeChefNotFound);
            }
            await _chefRequestRepository.RemoveAsync(requestFromDb);
        }

        public async Task<IEnumerable<RequestToBecomeChefDTO>> GetAllRequestsToBecomeChef()
        {
            var listRequests = await _chefRequestRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<RequestToBecomeChefDTO>>(listRequests);
            return result;
        }

        public async Task<RequestToBecomeChefDTO> GetRequestToBecomeChefById(string requestId)
        {
            var requestBecomeChef = await _chefRequestRepository.GetAsync(request => request.RequestChefId == requestId);

            var result = _mapper.Map<RequestToBecomeChefDTO>(requestBecomeChef);
            return result;
        }

        public async Task<RequestToBecomeChefDTO> GetRequestToBecomeChefByUserId(string userId)
        {
            var requestBecomeChef = await _chefRequestRepository.GetAsync(x => x.UserID == userId);

            var result = _mapper.Map<RequestToBecomeChefDTO>(requestBecomeChef);
            return result;
        }

        public async Task<RequestToBecomeChefDTO> ApprovalRequestByAdmin(ApprovalRequestDTO approvalRequestDTO)
        {
            var existingRequest = await _chefRequestRepository.GetAsync(req => req.RequestChefId == approvalRequestDTO.RequestId);
            if (existingRequest == null) {
                throw new InvalidDataException();
            }
            if (approvalRequestDTO.Status != ActionStatus_ACCEPTED && approvalRequestDTO.Status != ActionStatus_REJECTED)
            {
                throw new InvalidOperationException(AppString.InValidStatusType);
            }
            _mapper.Map(approvalRequestDTO, existingRequest);
            await _chefRequestRepository.UpdateRequestToBecomeChef(existingRequest);
            return _mapper.Map<RequestToBecomeChefDTO>(existingRequest);
        }
    }
}
