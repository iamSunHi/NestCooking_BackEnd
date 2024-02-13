using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using Newtonsoft.Json.Linq;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class OAuthRepository : IOAuthRepository
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public OAuthRepository(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<JObject> SignInWithFacebook(string accessToken)
		{
			var client = _httpClientFactory.CreateClient();

			var response = await client.GetAsync($"https://graph.facebook.com/me?fields=id,first_name,last_name,email,picture&access_token={accessToken}");

			if (!response.IsSuccessStatusCode)
				throw new Exception(AppString.InvalidTokenErrorMessage);

			var content = await response.Content.ReadAsStringAsync();
			var result = JObject.Parse(content);
			return result;
		}

		public async Task<JObject> SignInWithGoogle(string accessToken)
		{
			var client = _httpClientFactory.CreateClient();

			var response = await client.GetAsync($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={accessToken}");

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception(AppString.InvalidTokenErrorMessage);
			}

			var content = await response.Content.ReadAsStringAsync();
			var result = JObject.Parse(content);

			return result;
		}
	}
}
