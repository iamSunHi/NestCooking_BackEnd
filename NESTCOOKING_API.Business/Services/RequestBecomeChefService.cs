using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.Business.DTOs.NotificationDTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Business.Services
{
    public class RequestBecomeChefService : IRequestBecomeChefService
    {
        private readonly IRequestBecomeChefRepository _chefRequestRepository;
        private readonly UserManager<User> _userManager;
        private readonly IRoleRepository _roleRepository;
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RequestBecomeChefService(IRequestBecomeChefRepository chefRequestRepository, UserManager<User> userManager, IMapper mapper, INotificationService notificationService, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _chefRequestRepository = chefRequestRepository;
            _userManager = userManager;
            _mapper = mapper;
            _notificationService = notificationService;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
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
               
                if (user.RoleId == await _roleRepository.GetRoleIdByNameAsync(Role_Chef))
                {
                    throw new Exception(AppString.RequestNotification);
                }

                var requestToBecomeChef = _mapper.Map<RequestToBecomeChef>(requestToBecomeChefDTO);
                requestToBecomeChef.RequestChefId = Guid.NewGuid().ToString();
                requestToBecomeChef.UserID = userId;
                requestToBecomeChef.Status = ActionStatus_PENDING;
                requestToBecomeChef.ResponseId = null;
                requestToBecomeChef.CreatedAt = DateTime.Now;

                var result = this._mapper.Map<RequestToBecomeChefDTO>(await _chefRequestRepository.CreateRequestToBecomeChef(requestToBecomeChef));


                // notification 
                // co roleId
                var userList = await _userRepository.GetAllAsync();
                string roleIdAdmin = await _roleRepository.GetRoleIdByNameAsync(Role_Admin);
                var adminList = userList.Where(roleId => roleId.RoleId == roleIdAdmin).ToList();
                foreach (var ad in adminList)
                {

                    NotificationCreateDTO notificationCreateDTO = new NotificationCreateDTO
                    {
                        SenderId = userId,
                        ReceiverId = ad.Id,
                        Content = $"New Request Become Chef Form {userId}",
                        NotificationType = NotificationType_REQUEST,
                        TargetType = TargetType_REQUESTBECOMCHEF,
                    };
                    await _notificationService.CreateNotificationAsync(notificationCreateDTO);

                }
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
                existingRequest.CreatedAt = DateTime.Now;
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
            var existingRequest = await _chefRequestRepository.GetAsync(r => r.RequestChefId == approvalRequestDTO.RequestId);
            var userSendRequest = await _userManager.FindByIdAsync(existingRequest?.UserID);
            if (existingRequest == null)
            {
                throw new InvalidDataException();
            }

            if (approvalRequestDTO.Status != ActionStatus_ACCEPTED && approvalRequestDTO.Status != ActionStatus_REJECTED)
            {
                throw new InvalidOperationException(AppString.InValidStatusType);
            }

            // Check Status Request
            if (approvalRequestDTO.Status == ActionStatus_ACCEPTED)
            {
                userSendRequest.RoleId = await _roleRepository.GetRoleIdByNameAsync(Role_Chef);
            }
            else if (existingRequest.Status == ActionStatus_ACCEPTED && approvalRequestDTO.Status == ActionStatus_REJECTED)
            {
                userSendRequest.RoleId = await _roleRepository.GetRoleIdByNameAsync(Role_User);
            }

            existingRequest.Status = approvalRequestDTO.Status;

            var responseId = $"{Guid.NewGuid().ToString("N")}-{DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss")}";
            existingRequest.ResponseId = responseId;


            await _userManager.UpdateAsync(userSendRequest);
            await _chefRequestRepository.UpdateRequestToBecomeChef(existingRequest);

            var createNotificationDTO = new NotificationCreateDTO
            {
                SenderId = null,
                Content = approvalRequestDTO.Status == ActionStatus_ACCEPTED
               ? $"{userSendRequest.UserName} ! {AppString.NotificationAcceptedRequestBecomeChef}"
               : $"{AppString.NotificationRejectedRequestBecomChef}",
                NotificationType = NotificationType_ANNOUNCEMENT,
                ReceiverId = userSendRequest.Id,
                TargetType = TargetType_REQUESTBECOMCHEF
            };
            await _notificationService.CreateNotificationAsync(createNotificationDTO);

            return _mapper.Map<RequestToBecomeChefDTO>(existingRequest);
        }
       
    }
}
