using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.API.Helpers;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/user")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IUserConnectionService _userConnectionService;

		public UserController(IUserService userService, IUserConnectionService userConnectionService)
		{
			_userService = userService;
			_userConnectionService = userConnectionService;
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> GetUserById(string userId)
		{
			try
			{
				if (userId == null)
				{
					return BadRequest(ResponseDTO.BadRequest("UserId is required"));
				}
				var user = await _userService.GetUserById(userId);

				return Ok(ResponseDTO.Accept(result: user));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetInfo()
		{
			var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != null)
			{
				var userInfo = await _userService.GetUserById(userId);

				if (userInfo != null)
				{
					return Ok(ResponseDTO.Accept(result: userInfo));
				}
				else { return BadRequest(ResponseDTO.BadRequest()); }
			}

			return Unauthorized();
		}

		[HttpPatch("password")]
		[Authorize]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
		{
			if (changePasswordDTO.ConfirmPassword != changePasswordDTO.NewPassword)
			{
				return BadRequest(ResponseDTO.BadRequest(message: AppString.ConfirmPasswordMismatchErrorMessage));
			}

			if (changePasswordDTO.CurrentPassword == changePasswordDTO.NewPassword)
			{
				return BadRequest(ResponseDTO.BadRequest(message: AppString.SamePasswordErrorMessage));
			}

			var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != null)
			{
				var isPasswordChanged = await _userService.ChangePassword(userId, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword, changePasswordDTO.ConfirmPassword);

				if (isPasswordChanged)
				{
					return Ok(ResponseDTO.Accept(message: AppString.ChangePasswordSuccessMessage));
				}
				else
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.InvalidPasswordErrorMessage));
				}
			}

			return Unauthorized();
		}

		[HttpPatch]
		[Authorize]
		[ApiValidatorFilter]
		public async Task<IActionResult> EditProfile([FromBody] UpdateUserDTO updateUserDTO)
		{
			var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != null)
			{
				var user = await _userService.EditProfile(userId, updateUserDTO);

				if (user != null)
				{
					return Ok(ResponseDTO.Accept(message: AppString.UpdateInformationSuccessMessage, result: user));
				}
				else
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.UpdateInformationErrorMessage));
				}
			}

			return Unauthorized();
		}

		// [HttpGet("following")]
		// public async Task<IActionResult> GetAllFollowingUsersAsync()
		// {
		// 	try
		// 	{
		// 		var userId = GetUserIdFromContext(HttpContext);
		// 		var followedUserList = await _userConnectionService.GetAllFollowingUsersByUserIdAsync(userId);

		// 		return Ok(ResponseDTO.Accept(result: followedUserList));
		// 	}
		// 	catch (Exception ex)
		// 	{
		// 		return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
		// 	}
		// }

		// [HttpGet("followers")]
		// [Authorize]
		// public async Task<IActionResult> GetAllFollowersAsync()
		// {
		// 	try
		// 	{
		// 		var userId = GetUserIdFromContext(HttpContext);
		// 		var followedUserList = await _userConnectionService.GetAllFollowersByUserIdAsync(userId);

		// 		return Ok(ResponseDTO.Accept(result: followedUserList));
		// 	}
		// 	catch (Exception ex)
		// 	{
		// 		return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
		// 	}
		// }

		[HttpGet("following/{userId}")]
		public async Task<IActionResult> GetAllFollowingUsersByUserIdAsync([FromRoute] string userId)
		{
			try
			{
				var followedUserList = await _userConnectionService.GetAllFollowingUsersByUserIdAsync(userId);

				return Ok(ResponseDTO.Accept(result: followedUserList));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("followers/{userId}")]
		public async Task<IActionResult> GetAllFollowersByUserIdAsync([FromRoute] string userId)
		{
			try
			{
				var followedUserList = await _userConnectionService.GetAllFollowersByUserIdAsync(userId);

				return Ok(ResponseDTO.Accept(result: followedUserList));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPost("follow/{followingUserId}")]
		[Authorize]
		public async Task<IActionResult> CreateUserConnectionAsync(string followingUserId)
		{
			try
			{
				var userId = GetUserIdFromContext(HttpContext);
				if (userId == followingUserId)
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.UserCannotFollowSelf));
				}
				await _userConnectionService.CreateUserConnectionAsync(userId, followingUserId);

				return Created();
			}
			catch
			{
				return BadRequest(ResponseDTO.BadRequest(message: AppString.UserAlreadyFollowed));
			}
		}

		[HttpDelete("follow/{followingUserId}")]
		[Authorize]
		public async Task<IActionResult> RemoveUserConnectionAsync(string followingUserId)
		{
			try
			{
				var userId = GetUserIdFromContext(HttpContext);
				await _userConnectionService.RemoveUserConnectionAsync(userId, followingUserId);

				return NoContent();
			}
			catch
			{
				return BadRequest(ResponseDTO.BadRequest(message: AppString.UserNotFollowed));
			}
		}

		private string GetUserIdFromContext(HttpContext context)
		{
			return context.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
		}
	}
}

