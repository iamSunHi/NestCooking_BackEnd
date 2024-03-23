using NESTCOOKING_API.Business.DTOs.AdminDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IUserService
	{
		Task<List<AdminUserDTO>> GetAllUsersAsync();
		Task LockUserAsync(string userId, int minute);
		Task UnlockUserAsync(string userId);
		Task<UserDetailInfoDTO> GetUserById(string id);
		Task<bool> ChangePassword(string userId, string currentPassword, string newPassword, string confirPassword);
		Task<UserDetailInfoDTO> EditProfile(string userId, UpdateUserDTO updateUserDTO);
		bool IsUniqueEmail(string email);
		Task UpdateUserBalanceWithDeposit(string id, double amount);
		Task<bool> ChangeUserBalanceByTranPurchased(string userId, double amount, string recipeId);
		Task UpdateUserBalanceWithPurchaseRecipe(double amount, string recipeId);
		Task<bool> ChangeUserBalanceByWithdraw(string userId, double amount);
	}
}
