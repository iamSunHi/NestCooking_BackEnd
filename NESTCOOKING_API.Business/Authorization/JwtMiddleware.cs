using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NESTCOOKING_API.Business.Authorization
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;

		public JwtMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
		{
			var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
			if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
			{
				var token = authorizationHeader.Substring("Bearer ".Length);
				var userId = jwtUtils.ValidateJwtToken(token);

				if (userId != null)
				{
					var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, userId)
                };

					var identity = new ClaimsIdentity(claims, "jwt");
					context.User = new ClaimsPrincipal(identity);
				}
			}

			await _next(context);
		}
	}
}
