using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.Authorization;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AuthDTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IOAuthRepository _oAuthRepository;
        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IJwtUtils jwtUtils,
            UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager, IOAuthRepository oAuthRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtUtils = jwtUtils;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _oAuthRepository = oAuthRepository;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userRepository.Login(loginRequestDTO.UserName, loginRequestDTO.Password);

            if (user == null)
            {
                return null;
            }

            if (!_userManager.IsEmailConfirmedAsync(user).Result)
            {
                throw new EmailNotConfirmedException(AppString.NotEmailConfirmedErrorMessage);
            }
            if (_userManager.IsLockedOutAsync(user).Result)
            {
                throw new Exception(AppString.LockoutAccountErrorMessage);
            }

            LoginResponseDTO loginResponseDTO = new()
            {
                AccessToken = await _jwtUtils.GenerateJwtToken(user)
            };

            return loginResponseDTO;
        }

        public async Task<bool> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            if (!_userRepository.IsUniqueUserName(registrationRequestDTO.UserName))
            {
                throw new Exception("Your username is already exist!");
            }
            if (!_userRepository.IsUniqueEmail(registrationRequestDTO.Email))
            {
                throw new Exception("Your email is already exist!");
            }

            var newUser = _mapper.Map<User>(registrationRequestDTO);
            newUser.CreatedAt = DateTime.UtcNow.AddHours(7);
            var result = await _userRepository.Register(newUser, registrationRequestDTO.Password);

            return result;
        }

        public async Task<string> LoginWithThirdParty(OAuth2RequestDTO oAuth2RequestDTO, ProviderLogin providerLogin)
        {
            try
            {
                JObject oAuth2Profile = null;

                if (providerLogin == ProviderLogin.FACEBOOK)
                {
                    oAuth2Profile = await _oAuthRepository.SignInWithFacebook(oAuth2RequestDTO.AccessToken);
                }
                if (providerLogin == ProviderLogin.GOOGLE)
                {
                    oAuth2Profile = await _oAuthRepository.SignInWithGoogle(oAuth2RequestDTO.AccessToken);
                }

                if (oAuth2Profile == null)
                {
                    throw new Exception(AppString.InvalidTokenErrorMessage);
                }
                var info = CreateLoginWithThirdPartyRequest(oAuth2Profile, providerLogin);

                var user = await _userManager.FindByEmailAsync(info.Email);

                if (user == null)
                {
                    // Create a new user if not exists
                    if (!await _roleManager.RoleExistsAsync(StaticDetails.Role_User))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User));
                    }
                    var newUser = new User
                    {
                        UserName = info.Email,
                        FirstName = info.FirstName,
                        LastName = info.LastName,
                        Email = info.Email,
                        CreatedAt = DateTime.UtcNow.AddHours(7),
                        AvatarUrl = info.Picture,
                        RoleId = await _roleRepository.GetRoleIdByNameAsync(StaticDetails.Role_User)
                    };

                    var result = await _userManager.CreateAsync(newUser);
                    if (result.Succeeded)
                    {
                        user = await _userManager.FindByEmailAsync(info.Email);
                    }
                }

                if (user == null)
                {
                    return null;
                }

                // Auto confirm email when login with third party application
                await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));

                bool isLockedOut = await _userManager.IsLockedOutAsync(user);
                if (isLockedOut)
                {
                    return null;
                }
                return await _jwtUtils.GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<(string, string)> GenerateResetPasswordToken(string identifier)
        {
            User? user;

            if (Validation.CheckEmailValid(identifier))
            {
                user = await _userManager.FindByEmailAsync(identifier);
            }
            else
            {
                user = await _userManager.FindByNameAsync(identifier);
            }

            if (user == null)
            {
                throw new Exception(AppString.UserNotFoundMessage);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception(AppString.SomethingWrongMessage);
            }

            return (token, user.Email);

        }

        public async Task<bool> ResetPassword(ResetPasswordRequestDTO resetPasswordRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequestDTO.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, resetPasswordRequestDTO.Token, resetPasswordRequestDTO.NewPassword);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.ToList().FirstOrDefault().Description);
                }
                return true;
            }
            else
            {
                throw new Exception(AppString.UserNotFoundMessage);
            }
        }

        public async Task<bool> VerifyResetPasswordToken(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new Exception(AppString.UserNotFoundMessage);
            }

            return await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, UserManager<User>.ResetPasswordTokenPurpose, token); ;
        }

        public async Task<(string, string)> GenerateEmailConfirmationTokenAsync(string identifier)
        {
            User? user;
            var email = new EmailAddressAttribute();

            if (email.IsValid(identifier))
            {
                user = await _userManager.FindByEmailAsync(identifier);
            }
            else
            {
                user = await _userManager.FindByNameAsync(identifier);
            }

            if (user == null)
            {
                throw new Exception(AppString.UserNotFoundMessage);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception(AppString.SomethingWrongMessage);
            }

            return (email: user.Email, token);
        }

        public async Task<bool> VerifyEmailConfirmation(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new Exception(AppString.UserNotFoundMessage);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded) throw new Exception(AppString.InvalidTokenErrorMessage);

            return true;
        }

        public async Task<(string Email, string Username, string AvatarURL)> VerifyIdentifierResetPassword(string identifier)
        {
            User? user;

            if (Validation.CheckEmailValid(identifier))
            {
                user = await _userManager.FindByEmailAsync(identifier);
            }
            else
            {
                user = await _userManager.FindByNameAsync(identifier);
            }

            if (user == null)
            {
                throw new Exception(AppString.UserNotFoundMessage);
            }

            return (user.Email, user.UserName, user.AvatarUrl);
        }

        public async Task<bool> VerifyEmailResetPassword(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new Exception(AppString.UserNotFoundMessage);
            }

            var result = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, UserManager<User>.ResetPasswordTokenPurpose, token);

            if (!result) throw new Exception(AppString.InvalidTokenErrorMessage);

            return true;
        }


        private LoginWithThirdPartyRequestDTO CreateLoginWithThirdPartyRequest(JObject principal, ProviderLogin provider)
        {

            string firstNameKey = provider == ProviderLogin.FACEBOOK ? "first_name" : "given_name";
            string lastNameKey = provider == ProviderLogin.FACEBOOK ? "last_name" : "family_name";
            string pictureKey = provider == ProviderLogin.FACEBOOK ? "picture.data.url" : "picture";

            var pictureUrl = principal.SelectToken(pictureKey).ToString();
            return new LoginWithThirdPartyRequestDTO
            {
                FirstName = principal[firstNameKey]?.ToString(),
                LastName = principal[lastNameKey]?.ToString(),
                Email = principal["email"]?.ToString(),
                Picture = pictureUrl
            };
        }
    }
}
