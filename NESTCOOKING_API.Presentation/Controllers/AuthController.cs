using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.EmailDTO;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.ComponentModel.DataAnnotations;
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
		public AuthController(IAuthService authService, IEmailService emailService)
		{
			_authService = authService;
			_emailService = emailService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ResponseDTO.BadRequest(message: "Error in request!"));
			}
			try
			{
				var loginResponse = await _authService.Login(model);

				if (loginResponse == null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.IncorrectCredentialsLoginErrorMessage));
				}
				else if (string.IsNullOrEmpty(loginResponse.AccessToken))
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.AccountLockedOutLoginErrorMessage));
				}
				return Ok(ResponseDTO.Accept(result: loginResponse));
			}
			catch (EmailNotConfirmedException exception)
			{
				var (email, token) = await _authService.GenerateEmailConfirmationTokenAsync(model.UserName);

				//var emailConfirmationLink = Url.Action(nameof(VerifyEmailConfirmation), "auth", new { token, email = model.Email }, Request.Scheme);
				var emailConfirmationLink = $"{StaticDetails.FE_URL}/verify-email?token={token}&email={email}";

				_emailService.SendEmail(new EmailResponseDTO(
					to: new string[] { email },
					subject: AppString.ResendEmailConfirmationSubjectEmail,
					content: AppString.ResendEmailConfirmationContentEmail(emailConfirmationLink)
				));

				return BadRequest(ResponseDTO.BadRequest(message: exception.Message));
			}
			catch (Exception error)
			{
				return BadRequest(ResponseDTO.BadRequest(message: error.Message));
			}
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
		{

			if (!Validation.CheckEmailValid(model.Email))
			{
				return BadRequest(ResponseDTO.BadRequest(message: AppString.InvalidEmailErrorMessage));
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(ResponseDTO.BadRequest(message: AppString.RequestErrorMessage));
			}

			try
			{
				var isRegisted = await _authService.Register(model);
				if (isRegisted)
				{
					var (email, token) = await _authService.GenerateEmailConfirmationTokenAsync(model.Email);

					//var emailConfirmationLink = Url.Action(nameof(VerifyEmailConfirmation), "auth", new { token, email = model.Email }, Request.Scheme);
					var emailConfirmationLink = $"{StaticDetails.FE_URL}/verify-email?token={token}&email={email}";

					_emailService.SendEmail(new EmailResponseDTO(
						to: new string[] { model.Email },
						subject: AppString.EmailConfirmationSubjectEmail,
						content: AppString.EmailConfirmationContentEmail(emailConfirmationLink)
					));

					return Ok(ResponseDTO.Accept(message: AppString.RegisterSuccessMessage));
				}
				return BadRequest(ResponseDTO.BadRequest(message: AppString.RegisterErrorMessage));
			}
			catch (Exception error)
			{
				return BadRequest(ResponseDTO.BadRequest(message: error.Message));
			}
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

			var userProviderDTO = CreateProviderRequestDTO(result.Principal, Provider.Facebook);

			var token = await _authService.LoginWithThirdParty(userProviderDTO);

			if (token == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: "Authentication failed!"));
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
			var userProviderDTO = CreateProviderRequestDTO(result.Principal, Provider.Google);

			var token = await _authService.LoginWithThirdParty(userProviderDTO);

			if (token == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: "Authentication failed!"));
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
				ProviderKey = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value,
				ProviderDisplayName = principal.FindFirst(ClaimTypes.Name)?.Value,
				FirstName = principal.Claims.FirstOrDefault(filter => filter.Type == ClaimTypes.GivenName)?.Value,
				LastName = principal.Claims.FirstOrDefault(filter => filter.Type == ClaimTypes.Surname)?.Value,
				Email = principal.FindFirst(ClaimTypes.Email)?.Value,
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

		[HttpPost("reset-password/verify-identifier")]
		public async Task<IActionResult> VerifyIdentifierResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
		{
			try
			{
				if (string.IsNullOrEmpty(resetPasswordDTO.Identifier))
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.RequestErrorMessage));
				}

				var result = await _authService.VerifyIdentifierResetPassword(resetPasswordDTO.Identifier);
				if (!string.IsNullOrEmpty(result.Email))
				{
					return Ok(ResponseDTO.Accept(result: new
					{
						email = result.Email,
						username = result.Username,
						avatarURL = result.AvatarURL,
					}));
				}

				return BadRequest(ResponseDTO.BadRequest(message: AppString.SomethingWrongMessage));
			}
			catch (Exception error)
			{
				return BadRequest(ResponseDTO.BadRequest(message: error.Message));
			}
		}
		[HttpPost("reset-password/send-email")]
		public async Task<IActionResult> SendResetPasswordEmail([FromBody] ResetPasswordDTO resetPasswordDTO)
		{
			try
			{
				if (string.IsNullOrEmpty(resetPasswordDTO.Identifier))
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.RequestErrorMessage));
				}

				(string Token, string Email) = await _authService.GenerateResetPasswordToken(resetPasswordDTO.Identifier);
				if (!string.IsNullOrEmpty(Token))
				{
					string resetPasswordLink = $"{StaticDetails.FE_URL}/reset-password?token={Token}&email={Email}";

					var message = new EmailResponseDTO(new string[] { Email }, AppString.ResetPasswordSubjectEmail, AppString.ResetPasswordContentEmail(resetPasswordLink));

					_emailService.SendEmail(message);

					return Ok(ResponseDTO.Accept(message: AppString.ResetPasswordSendMailMessage, result: resetPasswordLink));
				}

				return BadRequest(ResponseDTO.BadRequest(message: AppString.SomethingWrongMessage));
			}
			catch (Exception error)
			{
				return BadRequest(ResponseDTO.BadRequest(message: error.Message));
			}
		}

		[HttpPost("reset-password/verify-token")]
		public async Task<IActionResult> VerifyEmailResetPassword([FromBody] VerifyEmailTokenRequestDTO verifyResetPasswordRequestDTO)
		{
			try
			{
				var isVerified = await _authService.VerifyEmailResetPassword(verifyResetPasswordRequestDTO.Email, verifyResetPasswordRequestDTO.Token);

				if (isVerified)
				{
					return Ok(ResponseDTO.Accept(message: AppString.EmailConfirmationSuccessMessage));
				}

				return BadRequest(ResponseDTO.BadRequest(message: AppString.SomethingWrongMessage));
			}
			catch (Exception exception)
			{
				return BadRequest(ResponseDTO.BadRequest(message: exception.Message));
			}
		}
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO resetPasswordRequestDTO)
		{
			try
			{
				if (resetPasswordRequestDTO == null)
				{
					return BadRequest(ResponseDTO.BadRequest());
				}


				if (resetPasswordRequestDTO.NewPassword != resetPasswordRequestDTO.ConfirmPassword)
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.ConfirmPasswordMismatchErrorMessage));
				}

				var isVerifiedToken = await _authService.VerifyResetPasswordToken(resetPasswordRequestDTO.Email, resetPasswordRequestDTO.Token);

				if (!isVerifiedToken)
				{
					return BadRequest(ResponseDTO.BadRequest(AppString.InvalidTokenErrorMessage));
				}

				var result = await _authService.ResetPassword(resetPasswordRequestDTO);
				if (result)
				{
					return Ok(ResponseDTO.Accept(AppString.ResetPasswordSuccessMessage));
				}
				return BadRequest(ResponseDTO.BadRequest(message: AppString.SomethingWrongMessage));
			}
			catch (Exception error)
			{
				return BadRequest(ResponseDTO.BadRequest(message: error.Message));
			}
		}

		[HttpPost("verify-email")]
		public async Task<IActionResult> VerifyEmailConfirmation([FromBody] VerifyEmailTokenRequestDTO verifyResetPasswordRequestDTO)
		{
			try
			{
				var isVerified = await _authService.VerifyEmailConfirmation(verifyResetPasswordRequestDTO.Email, verifyResetPasswordRequestDTO.Token);

				if (isVerified)
				{
					return Ok(ResponseDTO.Accept(message: AppString.EmailConfirmationSuccessMessage));
				}

				return BadRequest(ResponseDTO.BadRequest(message: AppString.SomethingWrongMessage));
			}
			catch (Exception exception)
			{
				return BadRequest(ResponseDTO.BadRequest(message: exception.Message));
			}
		}
	}
}
