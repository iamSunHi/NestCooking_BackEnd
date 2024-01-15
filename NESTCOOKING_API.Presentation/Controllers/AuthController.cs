using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Net;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		protected ResponseDTO _responseDTO;
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			this._responseDTO = new ResponseDTO();
			_authService = authService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
		{
			var loginResponse = await _authService.Login(model);

			if (loginResponse == null || string.IsNullOrEmpty(loginResponse.AccessToken))
			{
				_responseDTO.StatusCode = HttpStatusCode.BadRequest;
				_responseDTO.Message = "Username or password is incorrect!";
				_responseDTO.Result = null;
				return BadRequest(_responseDTO);
			}

			_responseDTO.StatusCode = HttpStatusCode.OK;
			_responseDTO.Message = "";
			_responseDTO.Result = loginResponse;

			return Ok(_responseDTO);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
		{
			if (ModelState.IsValid)
			{
				var registrationResponse = await _authService.Register(model);

				if (!string.IsNullOrEmpty(registrationResponse))
				{
					_responseDTO.StatusCode = HttpStatusCode.BadRequest;
					_responseDTO.Message = registrationResponse;
					_responseDTO.Result = null;
					return BadRequest(_responseDTO);
				}

				_responseDTO.StatusCode = HttpStatusCode.OK;
				_responseDTO.Message = "Successful account registration!";
				_responseDTO.Result = null;

				return Ok(_responseDTO);
			}

			_responseDTO.StatusCode = HttpStatusCode.BadRequest;
			_responseDTO.Message = "Error in request!";
			_responseDTO.Result = null;
			return BadRequest(_responseDTO);
		}

        [HttpGet("facebook")]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookCallback"),
                Items = { { "scheme", "Facebook" } }
            };

            return Challenge(properties, "Facebook");
        }



        [HttpGet("FacebookCallback")]
        public async Task<IActionResult> FacebookCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Facebook");

            if (!result.Succeeded)
            {

                return BadRequest("Facebook authentication failed.");
            }

            var userId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                UserId = userId,
                UserName = userName

            });
        }
    }
}
