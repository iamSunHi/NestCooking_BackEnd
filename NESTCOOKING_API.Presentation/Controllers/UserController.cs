using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/user")]
	[ApiController]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
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

		[HttpPatch("")]
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
	}
}

