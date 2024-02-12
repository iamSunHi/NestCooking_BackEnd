using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System.Net.Http;
using System.Security.Claims;
using static NESTCOOKING_API.Utility.StaticDetails;

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
                return null;
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

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception();
            }

            var userInfoDTO = await this.GetUserById(userId);

            return userInfoDTO;
        }
        public async Task<bool> ChangeAvatar(string userId, IFormFile file)
        {
            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory());

            var uploadPath = Path.Combine(currentDirectory, StaticDetails.AvatarFolderPath);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            var user = await _userManager.FindByIdAsync(userId);


            if (user.AvatarUrl != null)
            {
                var oldFilePath = Path.Combine(currentDirectory, user.AvatarUrl);
                File.Delete(oldFilePath);
            }
            user.AvatarUrl = Path.Combine(StaticDetails.AvatarFolderPath, fileName);


            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }
    }
}
