using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/requests")]
	[ApiController]
	[Authorize]
	public class RequestBecomeChefController : ControllerBase
	{
		private readonly IRequestBecomeChefService _userRequestService;

		public RequestBecomeChefController(IRequestBecomeChefService userRequestChef)
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
					return Ok(ResponseDTO.Accept(message: AppString.CreateRequestSuccessMessage));
				}
				return Unauthorized();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet]
		[Authorize(Roles = StaticDetails.Role_Admin)]
		public async Task<IActionResult> GetAllRequests()
		{
			try
			{
				var result = await _userRequestService.GetAllRequestsToBecomeChef();
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("{requestId}", Name = "GetRequestById")]
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
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("user/{userId}")]
		public async Task<IActionResult> GetRequestByUserId(string userId)
		{
			try
			{
				var request = await _userRequestService.GetRequestToBecomeChefByUserId(userId);

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
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("{requestId}")]
		public async Task<IActionResult> DeleteRequest(string requestId)
		{
			try
			{
				await _userRequestService.DeleteRequestToBecomeChef(requestId);
				return Ok(ResponseDTO.Accept(result: AppString.DeleteRequestSuccessMessage));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
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
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
