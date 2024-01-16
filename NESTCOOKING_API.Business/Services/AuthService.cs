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
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper _mapper;
		private string secretKey;
		public AuthService(IUserRepository userRepository, IConfiguration configuration,
			UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
		{
			_userRepository = userRepository;
			_userManager = userManager;
			_roleManager = roleManager;
			_mapper = mapper;
			secretKey = configuration.GetSection("ApiSettings:Secret").Value;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			var user = await _userRepository.Login(loginRequestDTO.UserName, loginRequestDTO.Password);

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
				AccessToken = tokenHandler.WriteToken(token),
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
						new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
						new Claim(ClaimTypes.Role, roles.FirstOrDefault())
					}),
					Expires = DateTime.UtcNow.AddDays(7),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
				};

				var token = tokenHandler.CreateToken(tokenDescriptor);

				return tokenHandler.WriteToken(token);
			}
			catch (Exception ex)
			{

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
						UserName = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(info.UserName)),
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
					Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
						new Claim(ClaimTypes.Role, roles.FirstOrDefault())
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

				return $"Error: {ex.Message}";
			}
		}

	}
}
