using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.Business.Services;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/requests")]
	[ApiController]
	[Authorize]
	public class RequestChefController : ControllerBase
	{
		private readonly IRequestBecomeChefService _userRequestService;

		public RequestChefController(IRequestBecomeChefService userRequestChef)
		{
			_userRequestService = userRequestChef;
		}

		[HttpPost]
		public async Task<IActionResult> CreateRequestBecomeChef([FromBody] CreatedRequestToBecomeChefDTO requestToBecomeChefDTO)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				if (userId != null)
				{

					var result = await _userRequestService.CreateRequestToBecomeChef(userId, requestToBecomeChefDTO);

					if (result != null)
					{
						return Ok(ResponseDTO.Accept(result: result));
					}
					else
					{
						return StatusCode(500, AppString.BecomeChefRequestInternalServerErrorMessage);
					}
				}
				return Unauthorized();
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllRequests()
		{
			try
			{
				var result = await _userRequestService.GetAllRequestsToBecomeChef();
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpGet("{requestId}")]
		public async Task<IActionResult> GetRequestById(string requestId)
		{
			try
			{
				var request = await _userRequestService.GetRequestToBecomeChefById(requestId);

				if (request != null)
				{
					return Ok(ResponseDTO.Accept(result: request));
				}
				else
				{
					return NotFound(AppString.RequestBecomeChefNotFound);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpDelete("{requestId}")]
		public async Task<IActionResult> DeleteRequest(string requestId)
		{
			try
			{
				var success = await _userRequestService.DeleteRequestToBecomeChef(requestId);

				if (success)
				{
					return Ok(ResponseDTO.Accept(result: AppString.DeleteRequestSuccessMessage));
				}
				else
				{
					return NotFound(AppString.RequestBecomeChefNotFound);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpPut("{requestId}")]
		public async Task<IActionResult> UpdateRequest(string requestId, [FromBody] CreatedRequestToBecomeChefDTO updatedRequestDTO)
		{
			try
			{
				var updatedRequest = await _userRequestService.UpdateRequestToBecomeChef(requestId, updatedRequestDTO);

				if (updatedRequest != null)
				{
					return Ok(ResponseDTO.Accept(AppString.UpdateRequestBecomeChefSuccessMessage, result: updatedRequest));
				}
				else
				{
					return NotFound(AppString.RequestBecomeChefNotFound);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}




	}




}
