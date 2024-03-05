using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.Authentication;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.NotificationDTOs;
using NESTCOOKING_API.Business.DTOs.ReactionDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/reaction")]
	[ApiController]
	public class ReactionController : ControllerBase
	{
		private readonly IReactionService _reactionService;
		private readonly INotificationService _notificationService;

		public ReactionController(IReactionService reactionService, INotificationService notificationService)
		{
			_reactionService = reactionService;
			_notificationService = notificationService;
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddReaction([FromBody] ReactionDTO reactionDTO)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				if (!String.Equals(reactionDTO.Type, StaticDetails.TargetType_RECIPE) && !String.Equals(reactionDTO.Type, StaticDetails.TargetType_COMMENT))
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Type is not valid"));
				}
				var result = await _reactionService.AddReactionAsync(reactionDTO, userId);
				if (result)
				{
					var notificationCreateDTO = new NotificationCreateDTO()
					{
						SenderId = userId,
						ReceiverId = reactionDTO.TargetID,
						NotificationType = StaticDetails.NotificationType_REACTION,
						TargetType = reactionDTO.Type,
					};
					await _notificationService.CreateNotificationAsync(notificationCreateDTO);

					return Ok(ResponseDTO.Accept("Add Reaction Success!"));
				}
				else
				{
					return BadRequest(ResponseDTO.BadRequest("Add Reaction Fail."));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("{type}")]
		[Authorize]
		public async Task<IActionResult> Delete(string type, [FromQuery] string targetId)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				if (string.IsNullOrEmpty(type))
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Type is required"));
				}
				if (!String.Equals(type, StaticDetails.TargetType_RECIPE) && !String.Equals(type, StaticDetails.TargetType_COMMENT))
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Type is not valid"));
				}
				if (string.IsNullOrEmpty(targetId))
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Target is required"));
				}
				var result = await _reactionService.DeleteReactionAsync(targetId, userId, type);
				if (result)
				{
					return Ok(ResponseDTO.Accept("Delete Reaction Success"));
				}
				else
				{
					return BadRequest(ResponseDTO.BadRequest("Delete Reaction Fail"));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPut]
		[Authorize]
		public async Task<IActionResult> UpdateReaction([FromBody] ReactionDTO reactionDTO)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				if (!String.Equals(reactionDTO.Type, StaticDetails.TargetType_RECIPE) && !String.Equals(reactionDTO.Type, StaticDetails.TargetType_COMMENT))

				{
					return BadRequest(ResponseDTO.BadRequest(message: "Type is not valid"));
				}
				var result = await _reactionService.UpdateReactionAsync(reactionDTO, userId);
				if (result)
				{
					return Ok(ResponseDTO.Accept(message: "Update Reaction Success"));
				}
				else
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Update Reaction Fail"));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("{type}")]
		public async Task<IActionResult> GetReactionsByTargetId(string type, [FromQuery] string targetId)
		{
			try
			{
				if (string.IsNullOrEmpty(type))
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Type is required"));
				}
				if (!String.Equals(type, StaticDetails.TargetType_RECIPE) && !String.Equals(type, StaticDetails.TargetType_COMMENT))
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Type is not valid"));
				}
				if (string.IsNullOrEmpty(targetId))
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Target is required"));
				}
				var result = await _reactionService.GetReactionsByIdAsync(targetId, type);
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("user/{type}")]
		[Authorize]
		public async Task<IActionResult> GetReactionUserById(string type)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				if (string.IsNullOrEmpty(type))
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Type is required"));
				}
				if (!String.Equals(type, StaticDetails.TargetType_RECIPE) && !String.Equals(type, StaticDetails.TargetType_COMMENT))
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Type is not valid"));
				}
				var result = await _reactionService.GetUserReaction(userId, type);
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
