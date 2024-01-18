using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.EmailDTO;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Security.Claims;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;
		private readonly IUserService _userService;
		public AuthController(IAuthService authService, IEmailService emailService, IConfiguration configuration, IUserService userService)
		{
			_authService = authService;
			_emailService = emailService;
			_configuration = configuration;
			_userService = userService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
		{
			var loginResponse = await _authService.Login(model);

			if (loginResponse == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: "Username or password is incorrect!"));
			}
			else if (string.IsNullOrEmpty(loginResponse.AccessToken))
			{
				return BadRequest(ResponseDTO.BadRequest(message: "This account is locked out!"));
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

			var token = await _authService.LoginWithThirdParty(userProviderDTO);

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

			var token = await _authService.LoginWithThirdParty(userProviderDTO);

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

		[HttpPost("verify-reset-password")]
		public async Task<IActionResult> ForgotPassword([FromBody] string identifier)
		{
			(string Token, string Email) result = await _authService.GenerateResetPasswordToken(identifier);
			if (!string.IsNullOrEmpty(result.Token))
			{
				var forgotPasswordLink = Url.Action(nameof(ResetPassword), "auth", new { result.Token, email = result.Email }, Request.Scheme);
				var message = new EmailResponseDTO(new string[] { result.Email! }, AppString.ResetPasswordSubjectEmail, AppString.ResetPasswordContentEmail(forgotPasswordLink));
				_emailService.SendEmail(message);

				return Ok(ResponseDTO.Accept(message: $"Password Changed request is sent on Email {result.Email}. Please Open Email And Click Link To Verify"));
			}

			return BadRequest(ResponseDTO.BadRequest(message: $"Could not send link. User not found. Please try again with a valid username or email."));
		}

		[HttpGet("reset-password")]
		public async Task<IActionResult> ResetPassword(string email, string token)
		{
			var isVerifiedToken = await _authService.VerifyResetPasswordToken(email, token);

			if (!isVerifiedToken)
			{
				return BadRequest(ResponseDTO.BadRequest("Something went wrong!"));
			}

			return Ok(ResponseDTO.Accept(result: new { token = token, email = email }));
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO resetPasswordRequestDTO)
		{
			if (resetPasswordRequestDTO == null)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}

			if (resetPasswordRequestDTO.NewPassword != resetPasswordRequestDTO.ConfirmPassword)
			{
				return BadRequest(ResponseDTO.BadRequest(message: "The 2 passwords must match!"));
			}

			var result = await _authService.ResetPassword(resetPasswordRequestDTO);
			if (string.IsNullOrEmpty(result))
			{
				return Ok(ResponseDTO.Accept());
			}
			return BadRequest(ResponseDTO.BadRequest($"{result}"));
		}
	}
}
