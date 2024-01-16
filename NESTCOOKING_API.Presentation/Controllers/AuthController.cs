using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Net;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.DataAccess.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Facebook;
using static NESTCOOKING_API.Utility.StaticDetails;
using Microsoft.IdentityModel.Tokens;

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

		[AllowAnonymous]
		[HttpGet("signin-facebook")]
		public IActionResult FacebookLogin()

		{
			var properties = new AuthenticationProperties
			{
				RedirectUri = Url.Action("FacebookCallback"),
				Items = { { "scheme", FacebookDefaults.AuthenticationScheme } }
			};

			return Challenge(properties, FacebookDefaults.AuthenticationScheme);
		}

		[AllowAnonymous]
		[HttpGet("facebook-response")]
		public async Task<IActionResult> FacebookCallback()
		{
			var result = await HttpContext.AuthenticateAsync("Facebook");
			if (!result.Succeeded)
			{
				return BadRequest(_responseDTO);
			}
			var userId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value; //ProviderKey
			var userName = result.Principal.FindFirst(ClaimTypes.Name)?.Value; // ProviderDisplayName
			var firstName = result.Principal.Claims.FirstOrDefault(filter => filter.Type == ClaimTypes.GivenName)?.Value;
			var lastName = result.Principal.Claims.FirstOrDefault(filter => filter.Type == ClaimTypes.Surname)?.Value;
			var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
			var address = result.Principal.FindFirst(ClaimTypes.StreetAddress)?.Value;
			var phoneNumber = result.Principal.FindFirst(ClaimTypes.MobilePhone)?.Value;

			var userFbDTO = new FacebookRequestDTO
			{
				LoginProvider = Provider.Facebook,
				ProviderKey = userId,
				ProviderDisplayName = userName,
				FirstName = firstName,
				LastName = lastName,
				Phone = phoneNumber,
				Email = email,
				Address = address,
			};

			var token = await _authService.LoginByFacebook(userFbDTO);

			if (token == null)
			{
				_responseDTO.StatusCode = HttpStatusCode.BadRequest;
				_responseDTO.Message = "Authentication failed";
				_responseDTO.Result = null;
				return BadRequest(_responseDTO);
			}

			LoginResponseDTO loginResponseDTO = new()
			{
				AccessToken = token
			};

			_responseDTO.Result = loginResponseDTO;
			_responseDTO.StatusCode = HttpStatusCode.OK;
			_responseDTO.Message = "";
			return Ok(_responseDTO);
		}

		[AllowAnonymous]
		[HttpGet("signin-google")]
		public IActionResult GoogleLogin()
		{
			var authenticationProperties = new AuthenticationProperties
			{
				RedirectUri = Url.Action("GoogleLoginCallback"),
				Items = { { "scheme", GoogleDefaults.AuthenticationScheme } }
			};


			return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
		}

		[AllowAnonymous]
		[HttpGet("google-response")]
		public async Task<IActionResult> GoogleLoginCallback()
		{
			var result = await HttpContext.AuthenticateAsync("Google");

			if (!result.Succeeded)
			{
				return BadRequest(_responseDTO);
			}
			var userId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value; // ProviderKey
			var userName = result.Principal.FindFirst(ClaimTypes.Name)?.Value; // ProviderDisplayName
			var firstName = result.Principal.Claims.FirstOrDefault(filter => filter.Type == ClaimTypes.GivenName)?.Value;
			var lastName = result.Principal.Claims.FirstOrDefault(filter => filter.Type == ClaimTypes.Surname)?.Value;
			var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
			var address = result.Principal.FindFirst(ClaimTypes.StreetAddress)?.Value;
			var phoneNumber = result.Principal.FindFirst(ClaimTypes.MobilePhone)?.Value;
			string[] nameParts = userName.Split(' ');

			var googleRequestDTO = new GoogleRequestDTO
			{
				LoginProvider = Utility.StaticDetails.Provider.Google,
				ProviderKey = userId,
				ProviderDisplayName = userName,
				FirstName = firstName,
				LastName = lastName,
				Phone = phoneNumber,
				Email = email,
				Address = address,
			};

			var token = await _authService.LoginByGoogle(googleRequestDTO);

			if (token == null)
			{
				_responseDTO.StatusCode = HttpStatusCode.BadRequest;
				_responseDTO.Message = "Authentication failed";
				_responseDTO.Result = null;
				return BadRequest(_responseDTO);
			}

			LoginResponseDTO loginResponseDTO = new()
			{
				AccessToken = token
			};

			_responseDTO.Result = loginResponseDTO;
			_responseDTO.StatusCode = HttpStatusCode.OK;
			_responseDTO.Message = "";
			return Ok(_responseDTO);

		}
	}
}
