using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Net;
using NESTCOOKING_API.Business.DTOs.EmailDTO;
using NESTCOOKING_API.Business.DTOs.ResetPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using NESTCOOKING_API.DataAccess.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Facebook;
using static NESTCOOKING_API.Utility.StaticDetails;
using Microsoft.AspNetCore.Routing;
using NESTCOOKING_API.Utility;
using Azure.Core;

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

        //[HttpGet("demo-send-email")]
        //public IActionResult SendEmail(string email, string content)
        //{

        //    var message = new EmailResponse(
        //        new string[] { email }, "NestCooking", content);
        //    _emailService.SendEmail(message);

        //    return StatusCode(StatusCodes.Status200OK,
        //        new ResponseDTO
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Message = "Email Send Successfully",
        //            Result = email
        //        }
        //        );

        //}

        [HttpPost("verify-reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required] string identifier)
        {

            User user = null;


            if (identifier.Contains("@")) // Validation for email
            {
                user = await _userService.GetUserByEmail(identifier);
            }
            else
            {
                user = await _userService.GetUserByUsername(identifier);
            }

            if (user != null)
            {
                var token = await _authService.GenerateResetPasswordToken(user);
                var forgotPasswordlink = Url.Action(nameof(ResetPassword), "auth", new { token, email = user.Email }, Request.Scheme);
                var message = new EmailResponse(new string[] { user.Email! }, AppString.ResetPasswordSubjectEmail, AppString.ResetPasswordContentEmail(forgotPasswordlink));
                _emailService.SendEmail(message);

                return Ok(ResponseDTO.Accept(message: $"Password Changed request is sent on Email {user.Email}. Please Open Email And Click Link To Verify"));
            }

            return BadRequest(ResponseDTO.BadRequest(message: $"Could not send link. User not found. Please try again with a valid username or email."));
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            User user = await _userService.GetUserByEmail(email);

            if (user == null)
            {
                return BadRequest(ResponseDTO.BadRequest("Invalid email address"));
            }

            var isVerifiedToken = await _authService.VerifyResetPasswordToken(user, token);

            if (!isVerifiedToken)
            {
                return BadRequest(ResponseDTO.BadRequest("Invalid reset password token"));
            }

            return Ok(ResponseDTO.Accept(result: new { token = token }));
        }


        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> SetPassword(ResetPassword resetPassword)
        {
            var user = await _userService.GetUserByEmail(resetPassword.Email);
            if (user != null)
            {
                var resetPasswordResult = await _authService.ResetPassword(user, resetPasswordToken: resetPassword.Token, resetPassword: resetPassword.Password);
                if (!resetPasswordResult.Succeeded)
                {
                    foreach (var error in resetPasswordResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }
                //     return StatusCode(StatusCodes.Status200OK,
                //    new ResponseDTO { StatusCode = HttpStatusCode.OK, Result = "Success", Message = $"Password Has Been Changed" });

                return Ok(ResponseDTO.Accept(result: "Success", message: $"Password Has Been Changed"));
            }
            else
            {
                return BadRequest(ResponseDTO.BadRequest(message: "Something went wrong"));
            }
            // return StatusCode(StatusCodes.Status400BadRequest,
            //    new ResponseDTO { StatusCode = HttpStatusCode.BadRequest, Result = "Error", Message = $"Could Not Send Link To email, Please try again" });
        }
    }
}
