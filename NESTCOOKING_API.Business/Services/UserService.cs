using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.Business.Services
{
	public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IRecipeRepository _recipeRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, UserManager<User> userManager, IMapper mapper, ITransactionRepository transactionRepository, IRecipeRepository recipeRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _recipeRepository = recipeRepository;

        }

        public Task<User> GetUserByEmail(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public Task<User> GetUserByUsername(string username)
        {
            return _userManager.FindByNameAsync(username);
        }

        public bool IsUniqueEmail(string email)
        {
            return this._userRepository.IsUniqueEmail(email);
        }

        public async Task<UserDetailInfoDTO> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException();
            }
            var userDTO = _mapper.Map<UserDetailInfoDTO>(user);
            userDTO.Role = await _roleRepository.GetRoleNameByIdAsync(user.RoleId);
            return userDTO;
        }

        public async Task<bool> ChangePassword(string userId, string currentPassword, string newPassword, string confirmPassword)
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

        public async Task<UserDetailInfoDTO> EditProfile(string userId, UpdateUserDTO updateUserDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            user.Address = updateUserDTO.Address;

            user.IsMale = updateUserDTO.IsMale;

            user.UserName = updateUserDTO.UserName;

            user.FirstName = updateUserDTO.FirstName;

            user.LastName = updateUserDTO.LastName;

            user.AvatarUrl = updateUserDTO.AvatarUrl;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception();
            }

            var userInfoDTO = await this.GetUserById(userId);

            return userInfoDTO;
        }
        public async Task ChangeUserBalanceByTranDeposit(string id, double amount)
        {
            try
            {
                var transaction = await _transactionRepository.GetAsync(t => t.Id == id);
                var user = await _userManager.FindByIdAsync(transaction.UserId);
                var updateSuccessful = await UpdateUserBalance(user, amount);

                if (updateSuccessful)
                {
                    await ChangeAdminBalance(amount);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user balance.", ex);
            }
        }
        public async Task<bool> ChangeUserBalanceByTranPurchased(string userId, double amount, string recipeId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var recipe = await _recipeRepository.GetAsync(t => t.Id == recipeId);
                var userRecipe = await _userManager.FindByIdAsync(recipe.UserId);

                if (user.Balance < amount)
                    return false;

                if (!await UpdateUserBalance(user, -amount))
                    return false;

                userRecipe.Balance += (amount * 0.9);
                if (!await UpdateUserBalance(userRecipe, amount * 0.9))
                {
                    await UpdateUserBalance(user, amount);
                    return false;
                }

                if (!await ChangeAdminBalance(amount * 0.1))
                {
                    await UpdateUserBalance(user, amount);
                    await UpdateUserBalance(userRecipe, -amount * 0.9);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error changing user balance.", ex);
            }
        }
        private async Task<bool> UpdateUserBalance(User user, double amount)
        {
            try
            {
                user.Balance += amount;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<bool> ChangeAdminBalance(double amount)
        {
            try
            {
                var adminUser = await _userManager.FindByNameAsync("admin");
                if (adminUser == null)
                    return false;

                return await UpdateUserBalance(adminUser, amount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
