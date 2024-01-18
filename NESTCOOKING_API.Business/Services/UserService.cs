using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
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

		public async Task<UserInfoDTO> GetUserById(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return null;
			}
			return _mapper.Map<UserInfoDTO>(user);
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

		public async Task<bool> EditProfile(string userId, UserInfoDTO userInfoDTO)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return false;
			}

			user.FirstName = userInfoDTO.FirstName;
			user.LastName = userInfoDTO.LastName;
			user.IsMale = userInfoDTO.IsMale;
			user.PhoneNumber = userInfoDTO.PhoneNumber;
			user.Email = userInfoDTO.Email;
			user.Address = userInfoDTO.Address;
			user.AvatarUrl = userInfoDTO.AvatarUrl;
			user.UpdatedAt = DateTime.UtcNow;

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
			{
				return false;
			}
			return true;
		}
	}

}
