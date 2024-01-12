using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NESTCOOKING_API.Business.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;
		private string secretKey;

		public UserService(IUserRepository userRepository, IConfiguration configuration,
			UserManager<User> userManager, IMapper mapper)
		{
			_userRepository = userRepository;
			_userManager = userManager;
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
	}
}
