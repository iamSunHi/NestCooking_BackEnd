using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/user")]
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
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO,string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null) {

                var userId = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

                if (userId != null)
                {

                    var passwordChanged = await _userService.ChangePassword(userId, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword, changePasswordDTO.ConnfirmPassword);

                    if (passwordChanged)
                    {
                        return Ok(ResponseDTO.Accept(message:"Password changed successfully."));
                    }
                    else
                    {
                        return BadRequest(ResponseDTO.BadRequest(message: "Invalid current password."));
                    }
                }
            }

            return Unauthorized();
        }
    }
}
