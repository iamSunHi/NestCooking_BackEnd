using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.Authorization;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

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

            LoginResponseDTO loginResponseDTO = new();
            if (!_userManager.IsLockedOutAsync(user).Result)
            {
                loginResponseDTO.AccessToken = await _jwtUtils.GenerateJwtToken(user);
            }

            return loginResponseDTO;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            if (!_userRepository.IsUniqueUserName(registrationRequestDTO.UserName))
            {
                return "Your username is already exist!";
            }
            if (!_userRepository.IsUniqueEmail(registrationRequestDTO.Email))
            {
                return "Your email is already exist!";
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

                var user = await _userManager.FindByLoginAsync(info.LoginProvider.ToString(), info.ProviderKey);

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

                if (_userManager.IsLockedOutAsync(user).Result)
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
        public async Task<(string, string)> GenerateResetPasswordToken(string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);

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
    }
}
