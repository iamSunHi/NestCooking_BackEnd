using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/user")]
	[Authorize]
	[ApiController]
	public class UserController : ControllerBase
	{
		protected ResponseDTO _responseDTO;
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			this._responseDTO = new ResponseDTO();
			_userService = userService;
		}

		[HttpPost("change-password")]
		public async Task<IActionResult> ChangePassword([FromHeader] string authorization, [FromBody] ChangePasswordDTO changePasswordDTO)
		{
			var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != null)
			{
				var isPasswordChanged = await _userService.ChangePassword(userId, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword, changePasswordDTO.ConfirmPassword);

				if (isPasswordChanged)
				{
					return Ok(ResponseDTO.Accept(message: "Password changed successfully."));
				}
				else
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Invalid password."));
				}
			}

			return Unauthorized();
		}

		[HttpPost("edit-profile")]
		public async Task<IActionResult> EditProfile([FromBody] UserInfoDTO userInfoDTO)
		{
			var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != null)
			{
				var isSuccess = await _userService.EditProfile(userId, userInfoDTO);

				if (isSuccess)
				{
					return Ok(ResponseDTO.Accept(message: "Your information changed successfully."));
				}
				else
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Something went wrong when update your information."));
				}
			}

			return Unauthorized();
		}
	}
}
