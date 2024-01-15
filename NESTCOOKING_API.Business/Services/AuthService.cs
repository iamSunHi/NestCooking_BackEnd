using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NESTCOOKING_API.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IProviderRepository _providerRepository;
        private string secretKey;
        public AuthService(IUserRepository userRepository, IConfiguration configuration,
            UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            secretKey = configuration.GetSection("ApiSettings:Secret").Value;
            _roleManager = roleManager;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userRepository.Login(loginRequestDTO.UserName, loginRequestDTO.Password);

            if (user == null)
            {
                return null;
            }

            var userDTO = _mapper.Map<UserDTO>(user);
            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT token
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponseDTO = new()
            {
                User = userDTO,
                Role = roles.FirstOrDefault(),
                Token = tokenHandler.WriteToken(token),
            };

            return loginResponseDTO;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            if (!_userRepository.IsUniqueUserName(registrationRequestDTO.UserName))
            {
                return "This username is already exist!";
            }

            var newUser = _mapper.Map<User>(registrationRequestDTO);
            newUser.CreatedAt = DateTime.UtcNow;
            var result = await _userRepository.Register(newUser, registrationRequestDTO.Password);

            return result;
        }
        public async Task<string> LoginByGoogle(GoogleRequestDTO info)
        {
            try
            {
                if (info == null)
                {
                    // Handle error
                    return null;
                }

                var user = await _userManager.FindByLoginAsync(info.LoginProvider.ToString(), info.ProviderKey);

                if (user == null)
                {
                    // Create a new user if not exists
                    var newUser = new User
                    {
                        UserName = info.Email,
                        Email = info.Email.ToString(),
                        CreatedAt = DateTime.UtcNow,
                        FirstName = info.FirstName,
                        LastName = info.LastName
                    };

                    var result = await _userManager.CreateAsync(newUser);

                    if (!await _roleManager.RoleExistsAsync(StaticDetails.Role_User))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User));
                    }
                    await _userManager.AddToRoleAsync(newUser, StaticDetails.Role_User);
                    if (result.Succeeded)
                    {
                        UserLoginInfo userLoginInfo = new UserLoginInfo(
                             info.LoginProvider.ToString(),
                             info.ProviderKey,
                             info.ProviderDisplayName);

                        result = await _userManager.AddLoginAsync(newUser, userLoginInfo);
                        if (result.Succeeded)
                        {
                            user = newUser;
                        }
                    }
                }

                if (user == null)
                {
                    return null;
                }

                var roles = await _userManager.GetRolesAsync(user);
                // Generate JWT token
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
    {
        new Claim(ClaimTypes.NameIdentifier, user?.Id?.ToString())
    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                // Add role claim if roles are available
                if (roles != null && roles.Any())
                {
                    var roleClaim = new Claim(ClaimTypes.Role, roles.FirstOrDefault());
                    tokenDescriptor.Subject.AddClaim(roleClaim);
                }

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return tokenString;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // For simplicity, we return an error message here
                return $"Error: {ex.Message}";
            }
        }
        public async Task<string> LoginByFacebook(FacebookRequestDTO info)
        {
            try
            {
                if (info == null)
                {
                    // Handle error
                    return null;
                }

                var user = await _userManager.FindByLoginAsync(info.LoginProvider.ToString(), info.ProviderKey);
                if (user == null)
                {
                    // Create a new user if not exists
                    var newUser = new User
                    {
                        UserName =WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(info.UserName)),
                        Email = info.Email,
                        CreatedAt = DateTime.UtcNow,
                        FirstName = info.FirstName,
                        LastName = info.LastName
                    };
                    var result = await _userManager.CreateAsync(newUser);
                    if (!await _roleManager.RoleExistsAsync(StaticDetails.Role_User))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User));
                    }
                    await _userManager.AddToRoleAsync(newUser, StaticDetails.Role_User);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddLoginAsync(newUser, new UserLoginInfo(
                            info.LoginProvider.ToString(),
                            info.ProviderKey,
                            info.ProviderDisplayName

                        ));
                        if (result.Succeeded)
                        {
                            user = newUser;
                        }
                    }
                }

                if (user == null)
                {
                    // Handle error
                    return null;
                }

                var roles = await _userManager.GetRolesAsync(user);
                // Generate JWT token
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]{
        new Claim(ClaimTypes.NameIdentifier, user?.Id?.ToString())}),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                // Add role claim if roles are available
                if (roles != null && roles.Any())
                {
                    var roleClaim = new Claim(ClaimTypes.Role, roles.FirstOrDefault());
                    tokenDescriptor.Subject.AddClaim(roleClaim);
                }

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return tokenString;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // For simplicity, we return an error message here
                return $"Error: {ex.Message}";
            }
        }

    }
}
