using NESTCOOKING_API.Business.DTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IUserService
	{
        Task<bool> ChangePassword(string userId, string currentPassword, string newPassword, string confirPassword);
    }
}
