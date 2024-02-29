using AutoMapper;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.Services
{
	public class UserConnectionService : IUserConnectionService
	{
		private readonly IUserConnectionRepository _userConnectionRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UserConnectionService(IUserConnectionRepository userConnectionRepository, IUserRepository userRepository, IMapper mapper)
		{
			_userConnectionRepository = userConnectionRepository;
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<List<UserShortInfoDTO>> GetAllFollowingUsersByUserIdAsync(string userId)
		{
			var userConnectionListFromDb = await _userConnectionRepository.GetAllAsync(uc => uc.UserId == userId);

			if (!userConnectionListFromDb.Any())
			{
				throw new Exception(AppString.UserNotHaveAnyConnection);
			}

			var followingUserList = new List<UserShortInfoDTO>();
			foreach (var userConnection in userConnectionListFromDb)
			{
				var user = await _userRepository.GetAsync(u => u.Id == userConnection.FollowingUserId);
				followingUserList.Add(_mapper.Map<UserShortInfoDTO>(user));
			}
			return followingUserList;
		}

		public async Task<List<UserShortInfoDTO>> GetAllFollowersByUserIdAsync(string userId)
		{
			var userConnectionListFromDb = await _userConnectionRepository.GetAllAsync(uc => uc.FollowingUserId == userId);

			if (!userConnectionListFromDb.Any())
			{
				throw new Exception(AppString.UserNotHaveAnyFollower);
			}

			var followerList = new List<UserShortInfoDTO>();
			foreach (var userConnection in userConnectionListFromDb)
			{
				var user = await _userRepository.GetAsync(u => u.Id == userConnection.UserId);
				followerList.Add(_mapper.Map<UserShortInfoDTO>(user));
			}
			return followerList;
		}

		public async Task CreateUserConnectionAsync(string userId, string followingUserId)
		{
			var userConnection = new UserConnection()
			{
				UserId = userId,
				FollowingUserId = followingUserId
			};
			await _userConnectionRepository.CreateAsync(userConnection);
		}

		public async Task RemoveAllFollowingUsersByUserIdAsync(string userId)
		{
			var userConnectionListFromDb = await _userConnectionRepository.GetAllAsync(uc => uc.UserId == userId);

			if (!userConnectionListFromDb.Any())
			{
				throw new Exception(AppString.UserNotHaveAnyConnection);
			}

			foreach (var userConnection in userConnectionListFromDb)
			{
				await _userConnectionRepository.RemoveAsync(userConnection);
			}
		}

		public async Task RemoveAllFollowersByUserIdAsync(string userId)
		{
			var userConnectionListFromDb = await _userConnectionRepository.GetAllAsync(uc => uc.FollowingUserId == userId);

			if (!userConnectionListFromDb.Any())
			{
				throw new Exception(AppString.UserNotHaveAnyFollower);
			}

			foreach (var userConnection in userConnectionListFromDb)
			{
				await _userConnectionRepository.RemoveAsync(userConnection);
			}
		}

		public async Task RemoveUserConnectionAsync(string userId, string followingUserId)
		{
			var userConnectionFromDb = await _userConnectionRepository.GetAsync(uc => uc.UserId == userId && uc.FollowingUserId == followingUserId);
			await _userConnectionRepository.RemoveAsync(userConnectionFromDb);
		}
	}
}
