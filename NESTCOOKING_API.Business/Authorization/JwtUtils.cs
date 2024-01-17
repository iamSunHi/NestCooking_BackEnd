using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NESTCOOKING_API.DataAccess.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NESTCOOKING_API.Business.Authorization
{
	public interface IJwtUtils
	{
		Task<string> GenerateJwtToken(User user);
		string? ValidateJwtToken(string? token);
	}

	public class JwtUtils : IJwtUtils
	{
		private readonly UserManager<User> _userManager;
		private readonly string secretKey;

		public JwtUtils(UserManager<User> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			secretKey = configuration.GetSection("ApiSettings:Secret").Value;
		}

		public async Task<string> GenerateJwtToken(User user)
		{
			var roles = await _userManager.GetRolesAsync(user);

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

		public string? ValidateJwtToken(string? token)
		{
			if (token == null)
				return null;

			var key = Encoding.ASCII.GetBytes(secretKey);
			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out var validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

				// return user's id from JWT token if validation successful
				if (!string.IsNullOrEmpty(userId))
				{
					return userId;
				}
			}
			catch
			{
			}
			return null;
		}
	}
}
