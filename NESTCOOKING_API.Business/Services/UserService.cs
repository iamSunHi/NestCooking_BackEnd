using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.Business.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;

		public UserService(UserManager<User> userManager)
		{
			_userManager = userManager;
		}
		public async Task<bool> ChangePassword(string userId, string currentPassword, string newPassword, string confirPassword)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return false;
			}

			var result = await _userManager.ChangePasswordAsync(user,
				currentPassword, newPassword);

			if (!result.Succeeded)
			{
				return false;
			}
			return true;
		}
	}

}
