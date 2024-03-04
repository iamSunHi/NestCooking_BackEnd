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

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, UserManager<User> userManager, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _mapper = mapper;

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
                throw new UserNotFoundException();
            }
            var userDTO = _mapper.Map<UserInfoDTO>(user);
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

        public async Task<UserInfoDTO> EditProfile(string userId, UpdateUserDTO updateUserDTO)
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
	}
}
