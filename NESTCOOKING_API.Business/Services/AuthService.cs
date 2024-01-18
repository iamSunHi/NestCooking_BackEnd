using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
                // Handle error
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
                        Email = info.Email.ToString(),
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
        public Task<string> GenerateResetPasswordToken(User user)
        {
            return _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPassword(User user, string resetPasswordToken, string resetPassword)
        {
            return await _userManager.ResetPasswordAsync(user, resetPasswordToken, resetPassword);
        }

        public async Task<bool> VerifyResetPasswordToken(User user, string token)
        {

            var result = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, UserManager<User>.ResetPasswordTokenPurpose, token);

            return result;
        }
    }
}
