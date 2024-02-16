using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Business.Services
{
    public class RequestBecomeChefService : IRequestBecomeChefService
    {
        private readonly IChefRequestRepository _chefRequestRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RequestBecomeChefService(IChefRequestRepository chefRequestRepository, UserManager<User> userManager, IMapper mapper)
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
                var requestToBecomeChef = _mapper.Map<RequestToBecomeChef>(requestToBecomeChefDTO);
                requestToBecomeChef.RequestChefId = Guid.NewGuid().ToString();
                requestToBecomeChef.UserID = userId;
                requestToBecomeChef.Status = ActionStatus_PENDING;
                requestToBecomeChef.ResponseId = null;
                requestToBecomeChef.CreatedAt = DateTime.Now;

                var createdRequest = await _chefRequestRepository.CreateRequestToBecomeChef(requestToBecomeChef);

                var result = _mapper.Map<RequestToBecomeChefDTO>(createdRequest);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(AppString.SomethingWrongMessage);
            }
        }
        public async Task<RequestToBecomeChefDTO> UpdateRequestToBecomeChef(string requestId, CreatedRequestToBecomeChefDTO requestToBecomeChefDTO)
        {
            var existingRequest = await _chefRequestRepository.GetRequestById(requestId);

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
        public async Task<bool> DeleteRequestToBecomeChef(string requestId)
        {
            return await _chefRequestRepository.DeleteRequestToBecomeChef(requestId);

        }
        public async Task<IEnumerable<RequestToBecomeChefDTO>> GetAllRequestsToBecomeChef()
        {
            var listRequests = await _chefRequestRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<RequestToBecomeChefDTO>>(listRequests);
            return result;
        }
        public async Task<RequestToBecomeChefDTO> GetRequestToBecomeChefById(string requestId)
        {
            var requestBecomeChef = await _chefRequestRepository.GetRequestById(requestId);

            var result = _mapper.Map<RequestToBecomeChefDTO>(requestBecomeChef);
            return result;
        }

    }
}
