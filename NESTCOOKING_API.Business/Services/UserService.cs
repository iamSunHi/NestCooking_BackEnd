using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
