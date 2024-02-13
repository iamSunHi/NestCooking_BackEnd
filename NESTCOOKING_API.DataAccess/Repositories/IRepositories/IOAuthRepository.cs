using NESTCOOKING_API.DataAccess.Models;
using Newtonsoft.Json.Linq;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IOAuthRepository
    {
        Task<JObject> SignInWithGoogle(String accessToken);
        Task<JObject> SignInWithFacebook(String accessToken);
    }
}
