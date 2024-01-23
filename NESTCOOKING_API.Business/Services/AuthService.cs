using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.Authorization;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public AuthService(IUserRepository userRepository, IJwtUtils jwtUtils,
            UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
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
            newUser.CreatedAt = DateTime.UtcNow;
            var result = await _userRepository.Register(newUser, registrationRequestDTO.Password);

            return result;
        }

        public async Task<string> LoginWithThirdParty(ProviderRequestDTO info)
        {
            try
            {
                if (info == null)
                {
                    return null;
                }

                var user = await _userManager.FindByEmailAsync(info.Email);

                if (user == null)
                {
                    // Create a new user if not exists
                    var newUser = new User
                    {
                        UserName = info.Email,
                        FirstName = info.FirstName,
                        LastName = info.LastName,
                        Email = info.Email,
                        CreatedAt = DateTime.UtcNow,
                    };

                    var result = await _userManager.CreateAsync(newUser);
                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(StaticDetails.Role_User))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User));
                        }
                        await _userManager.AddToRoleAsync(newUser, StaticDetails.Role_User);
                        await _userManager.AddLoginAsync(newUser, new UserLoginInfo(
                            info.LoginProvider.ToString(),
                            info.ProviderKey,
                            info.ProviderDisplayName
                        ));
                        user = await _userManager.FindByEmailAsync(info.Email);
                    }
                }
                else
                {
                    await _userManager.AddLoginAsync(
                        user,
                        new UserLoginInfo(
                            info.LoginProvider.ToString(),
                            info.ProviderKey,
                            info.ProviderDisplayName
                        )
                    );
                }

                if (user == null)
                {
                    return null;
                }

                bool isLockedOut = await _userManager.IsLockedOutAsync(user);

				if (isLockedOut)
                {
                    return null;
                }
                return await _jwtUtils.GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public async Task<(string, string)> GenerateResetPasswordToken(string identifier)
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

			if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                if (!string.IsNullOrEmpty(token))
                {
                    return (token, user.Email);
                }
            }

            return (null, null);
        }

        public async Task<string> ResetPassword(ResetPasswordRequestDTO resetPasswordRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequestDTO.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, resetPasswordRequestDTO.Token, resetPasswordRequestDTO.NewPassword);
                if (!result.Succeeded)
                {
                    return result.Errors.ToList().FirstOrDefault().Description;
                }
                return "";
            }
            else
            {
                return "Something went wrong!";
            }
        }

        public async Task<bool> VerifyResetPasswordToken(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
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
    }
}
