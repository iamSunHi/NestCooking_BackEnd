using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NESTCOOKING_API.Business.Authorization;
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
		private readonly IJwtUtils _jwtUtils;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper _mapper;
		private string secretKey;
		public AuthService(IUserRepository userRepository, IJwtUtils jwtUtils, IConfiguration configuration,
			UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
		{
			_userRepository = userRepository;
			_jwtUtils = jwtUtils;
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
				return null;
			}

			LoginResponseDTO loginResponseDTO = new()
			{
				AccessToken = await _jwtUtils.GenerateJwtToken(user)
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

		public async Task<string> LoginByGoogle(ProviderRequestDTO info)
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

				return await _jwtUtils.GenerateJwtToken(user);
			}
			catch (Exception ex)
			{

				return $"Error: {ex.Message}";
			}
		}

		public async Task<string> LoginByFacebook(ProviderRequestDTO info)
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
						UserName = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(info.Email)),
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
					return null;
				}

				return await _jwtUtils.GenerateJwtToken(user); ;
			}
			catch (Exception ex)
			{

				return $"Error: {ex.Message}";
			}
		}
	}
}
