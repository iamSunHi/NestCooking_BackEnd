using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NESTCOOKING_API.Business.Authentication
{
    public class AuthenticationHelper
    {
        public static string GetUserIdFromContext(HttpContext context)
        {
            return context.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}