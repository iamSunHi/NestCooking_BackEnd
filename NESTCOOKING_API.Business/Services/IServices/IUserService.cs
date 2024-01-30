using Microsoft.AspNetCore.Http;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IUserService
	{
		Task<UserInfoDTO> GetUserById(string id);
		Task<bool> ChangePassword(string userId, string currentPassword, string newPassword, string confirPassword);
		Task<bool> EditProfile(string userId, UserInfoDTO userInfoDTO);
		bool IsUniqueEmail(string email);
		Task<User> GetUserByEmail(string email);
		Task<User> GetUserByUsername(string username);
		Task<bool> ChangeAvatar(string userId, IFormFile file);
	}
}
