using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Facebook;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
		{
			var loginResponse = await _authService.Login(model);

			if (loginResponse == null || string.IsNullOrEmpty(loginResponse.AccessToken))
			{
				return BadRequest(ResponseDTO.BadRequest(message: "Username or password is incorrect!"));
			}
			return Ok(ResponseDTO.Accept(result: loginResponse));
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
		{
			if (ModelState.IsValid)
			{
				var registrationResponse = await _authService.Register(model);

				if (!string.IsNullOrEmpty(registrationResponse))
				{
					return BadRequest(ResponseDTO.BadRequest(message: registrationResponse));
				}

				return Ok(ResponseDTO.Accept(message: registrationResponse));
			}
			return BadRequest(ResponseDTO.BadRequest(message: "Error in request!"));
		}

		[AllowAnonymous]
		[HttpGet("signin-facebook")]
		public IActionResult FacebookLogin()

		{
			var authenticationProperties = CreateAuthenticationProperties("FacebookCallback", FacebookDefaults.AuthenticationScheme);

			return Challenge(authenticationProperties, FacebookDefaults.AuthenticationScheme);
		}

		[AllowAnonymous]
		[HttpGet("facebook-response")]
		public async Task<IActionResult> FacebookCallback()
		{
			var result = await HttpContext.AuthenticateAsync("Facebook");
			if (!result.Succeeded)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}

			var userProviderDTO = this.CreateProviderRequestDTO(result.Principal, Provider.Facebook);

			var token = await _authService.LoginByFacebook(userProviderDTO);

			if (token == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: "Authentication failed"));
			}

			LoginResponseDTO loginResponseDTO = new()
			{
				AccessToken = token
			};

			return Ok(ResponseDTO.Accept(result: loginResponseDTO));
		}

		[AllowAnonymous]
		[HttpGet("signin-google")]
		public IActionResult GoogleLogin()
		{

			var authenticationProperties = CreateAuthenticationProperties(redirectUri: "GoogleLoginCallback", scheme: GoogleDefaults.AuthenticationScheme);
			return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
		}

		[AllowAnonymous]
		[HttpGet("google-response")]
		public async Task<IActionResult> GoogleLoginCallback()
		{
			var result = await HttpContext.AuthenticateAsync("Google");

			if (!result.Succeeded)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}
			var userProviderDTO = this.CreateProviderRequestDTO(result.Principal, Provider.Google);

			var token = await _authService.LoginByGoogle(userProviderDTO);

			if (token == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: "Authentication failed"));
			}

			LoginResponseDTO loginResponseDTO = new()
			{
				AccessToken = token
			};

			return Ok(ResponseDTO.Accept(result: loginResponseDTO));

		}

		private ProviderRequestDTO CreateProviderRequestDTO(ClaimsPrincipal principal, Provider provider)
		{
			return new ProviderRequestDTO
			{
				LoginProvider = provider,
				ProviderKey = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
				ProviderDisplayName = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
				FirstName = principal.Claims.FirstOrDefault(filter => filter.Type == ClaimTypes.GivenName)?.Value ?? "",
				LastName = principal.Claims.FirstOrDefault(filter => filter.Type == ClaimTypes.Surname)?.Value ?? "",
				Phone = principal.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "",
				Email = principal.FindFirst(ClaimTypes.Email)?.Value ?? "",
				Address = principal.FindFirst(ClaimTypes.StreetAddress)?.Value ?? "",
			};
		}


		private AuthenticationProperties CreateAuthenticationProperties(string redirectUri, string scheme)
		{
			return new AuthenticationProperties
			{
				RedirectUri = Url.Action(redirectUri),
				Items = { { "scheme", scheme } }
			};
		}
	}
}
