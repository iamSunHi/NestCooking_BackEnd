using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
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
		private readonly IUserRequestChef _userRequestChef;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public RequestBecomeChefService(IUserRequestChef userRequestChef , UserManager<User> userManager, IMapper mapper)
        {
			_userRequestChef = userRequestChef;
			_userManager = userManager;
			_mapper = mapper;
        }
        public async Task<RequestToBecomeChef> CreateRequestToBecomeChef(string userId, RequestToBecomeChefDTO requestToBecomeChefDTO)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId);
				if(user == null)
				{
					throw new Exception(AppString.SomethingWrongMessage);
				}
				var requestToBecomeChef = _mapper.Map<RequestToBecomeChef>(requestToBecomeChefDTO);
				requestToBecomeChef.UserID = userId;
				requestToBecomeChef.RequestChefId = Guid.NewGuid().ToString();
				requestToBecomeChef.Status = RequestStatus.Pending;
				requestToBecomeChef.ResponseId = null;
				requestToBecomeChef.createAt = DateTime.Now;
				var createdRequest = await _userRequestChef.CreateRequestToBecomeChef(requestToBecomeChef);
				return createdRequest;
			}
			catch (Exception ex)
			{
				throw new Exception(AppString.SomethingWrongMessage);
			}
		}
		public async Task<RequestToBecomeChefDTO> UpdateRequestToBecomeChef(string requestId, RequestToBecomeChefDTO requestToBecomeChefDTO)
		{
			var existingRequest = await _userRequestChef.GetRequestById(requestId);

			if (existingRequest != null)
			{
				existingRequest.createAt = DateTime.Now;
				_mapper.Map(requestToBecomeChefDTO, existingRequest);
				await _userRequestChef.UpdateRequestToBecomeChef(existingRequest);
				var updatedDto = _mapper.Map<RequestToBecomeChefDTO>(existingRequest);
				return updatedDto;
			}
			return null;
		}
		public async Task<bool> DeleteRequestToBecomeChef(string requestId)
		{
			return await _userRequestChef.DeleteRequestToBecomeChef(requestId);

		}
		public async Task<IEnumerable<RequestToBecomeChef>> GetAllRequestsToBecomChef()
		{
			var listRequests = await _userRequestChef.GetAllRequests();
			return listRequests.ToList();
		}
		public async Task<RequestToBecomeChef> GetRequestToBecomeChefById(string requestId)
		{
				var requestBecomeChef = await _userRequestChef.GetRequestById(Guid.Parse(requestId).ToString()); 
				return requestBecomeChef;
		}
		public void ValidateImageUrls(List<string> imageUrls)
		{
			foreach (var imageUrl in imageUrls)
			{
				if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uriResult) || uriResult.Scheme != Uri.UriSchemeHttp)
				{
					throw new ArgumentException($"Invalid image URL: {imageUrl}");
				}
			}
		}
		public void ValidateRequestDTO(RequestToBecomeChefDTO requestDTO)
		{
			if (string.IsNullOrWhiteSpace(requestDTO.FullName))
			{
				throw new ArgumentException("Full name cannot be empty or whitespace.");
			}
			ValidateImageUrls(requestDTO.AchievementImageUrls);
		}

	
	}
}
