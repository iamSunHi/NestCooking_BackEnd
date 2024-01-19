using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IUserService
	{
		Task<UserInfoDTO> GetUserById(string id);
        Task<bool> ChangePassword(string userId, string currentPassword, string newPassword, string confirPassword);
		Task<bool> EditProfile(string userId, UserInfoDTO userInfoDTO);

		Task<bool> ChangeAvatar(string userId, IFormFile file);
    }
}
