using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/user")]
	[ApiController]
	[Authorize]
	public class UserController : ControllerBase
	{
		protected ResponseDTO _responseDTO;
		private readonly IUserService _userService;
     
        public UserController(IUserService userService,IConfiguration configuration)
        {
            this._responseDTO = new ResponseDTO();
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

		[HttpPost("change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
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

        [HttpPost("change-avatar")]
        public async Task<IActionResult> ChangeAvatar(IFormFile file)
        {
            try
            {
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;				

                if (userId != null)
				{
					var result = await _userService.ChangeAvatar(userId, file);
					if (result == true)
					{
						return Ok(ResponseDTO.Accept());
					}
					else
					{
						return BadRequest(ResponseDTO.BadRequest());
					}
				}
				return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

