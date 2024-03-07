using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

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
        public async Task UpdateUserBalanceWithDeposit(string transactionId, double amount)
        {
            try
            {
                var transaction = await _transactionRepository.GetAsync(t => t.Id == transactionId);
                var user = await _userManager.FindByIdAsync(transaction.UserId);
                await UpdateUserBalance(user, amount);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user balance.", ex);
            }
        }
        public async Task UpdateUserBalanceWithPurchaseRecipe(double amount, string recipeId)
        {
            var recipe = await _recipeRepository.GetAsync(t => t.Id == recipeId);
            var userRecipe = await _userManager.FindByIdAsync(recipe.UserId);
            await UpdateUserBalance(userRecipe, amount * 0.9);
            await ChangeAdminBalance(amount * 0.1);
        }

        public async Task<bool> ChangeUserBalanceByTranPurchased(string userId, double amount, string recipeId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var recipe = await _recipeRepository.GetAsync(t => t.Id == recipeId);
                var userCreatedRecipe = await _userManager.FindByIdAsync(recipe.UserId);

                // if (user.Balance < amount)
                //     return false;

                // if (!await UpdateUserBalance(user, -amount))
                //     return false;
                // if (!await UpdateUserBalance(userCreatedRecipe, amount * 0.9))
                // {
                //     await UpdateUserBalance(user, amount);
                //     return false;
                // }

                // if (!await ChangeAdminBalance(amount * 0.1))
                // {
                //     await UpdateUserBalance(user, amount);
                //     await UpdateUserBalance(userCreatedRecipe, -amount * 0.9);
                //     return false;
                // }

                // return true;
                var updateUserBalanceResult = await UpdateUserBalance(user, -amount);
                var updateUserRecipeBalanceResult = await UpdateUserBalance(userCreatedRecipe, amount * 0.9);
                var changeAdminBalanceResult = await ChangeAdminBalance(amount * 0.1);

                if (updateUserBalanceResult && updateUserRecipeBalanceResult && changeAdminBalanceResult)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("One or more balance update operations failed.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error changing user balance.", ex);
            }
        }
        public async Task<bool> ChangeUserBalanceByWithdraw(string userId, double amount)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user.Balance < amount)
                {
                    throw new Exception("The amount of money in the wallet is not enough to make the transaction");
                }

                if (!await UpdateUserBalance(user, -amount))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                var roleAdminId = await _roleRepository.GetRoleIdByNameAsync(StaticDetails.Role_Admin);
                var adminUser = await _userRepository.GetAsync(r => r.RoleId == roleAdminId);
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
