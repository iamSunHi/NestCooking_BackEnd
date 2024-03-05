using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IUserService
	{
		Task<UserDetailInfoDTO> GetUserById(string id);
		Task<bool> ChangePassword(string userId, string currentPassword, string newPassword, string confirPassword);
		Task<UserDetailInfoDTO> EditProfile(string userId, UpdateUserDTO updateUserDTO);
		bool IsUniqueEmail(string email);
		Task<User> GetUserByEmail(string email);
		Task<User> GetUserByUsername(string username);
		Task ChangeUserBalanceByTranDeposit(string id, double amount);
        Task<bool> ChangeUserBalanceByTranPurchased(string userId, double amount, string recipeId);
    }
}
