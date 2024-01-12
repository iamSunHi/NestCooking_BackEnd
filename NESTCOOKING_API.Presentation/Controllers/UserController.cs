using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Net;

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

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
		{
			var loginResponse = await _userService.Login(model);

			if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
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
				var registrationResponse = await _userService.Register(model);

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
	}
}
