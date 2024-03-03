using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.NotificationDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System.Drawing.Printing;

namespace NESTCOOKING_API.Business.Services
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationRepository _notificationRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository,
			IMapper mapper)
		{
			_notificationRepository = notificationRepository;
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<(int, int, IEnumerable<NotificationReadDTO>)> GetAllNotificationsWithPaginationAsync(PaginationInfoDTO paginationInfo)
		{
			var notificationListFromDb = await _notificationRepository.GetAllAsync();
			var totalItems = notificationListFromDb.Count();
			var totalPages = (int)Math.Ceiling((double)totalItems / paginationInfo.PageSize);

			notificationListFromDb = notificationListFromDb
				.Skip((paginationInfo.PageNumber - 1) * paginationInfo.PageSize)
				.Take(paginationInfo.PageSize);

			if (!notificationListFromDb.Any())
			{
				return (totalItems, totalPages, null);
			}

			return (totalItems, totalPages, _mapper.Map<IEnumerable<NotificationReadDTO>>(notificationListFromDb));
		}

		public async Task<IEnumerable<NotificationReadDTO>> GetAllNotificationsByReceiverIdAsync(string receiverId)
		{
			var receiver = await _userRepository.GetAsync(u => u.Id == receiverId);
			var notificationListFromDb = (await _notificationRepository
				.GetAllAsync(n => (n.ReceiverId == receiverId || n.SenderId == null) && n.CreatedAt >= receiver.CreatedAt))
				.ToList();

			if (!notificationListFromDb.Any())
			{
				throw new Exception("You don't have any notification.");
			}

			notificationListFromDb = notificationListFromDb.OrderByDescending(n => n.CreatedAt).ToList();

			var result = _mapper.Map<IEnumerable<NotificationReadDTO>>(notificationListFromDb).ToList();
			for (int i = 0; i < result.Count(); i++)
			{
				if (notificationListFromDb[i].SenderId != null)
				{
					var sender = await _userRepository.GetAsync(u => u.Id == notificationListFromDb[i].SenderId);
					result[i].Sender = _mapper.Map<UserShortInfoDTO>(sender);
				}
			}

			return result;
		}

		public async Task CreateNotificationAsync(NotificationCreateDTO notificationCreateDTO)
		{
			var notificationToDb = _mapper.Map<Notification>(notificationCreateDTO);
			notificationToDb.Id = Guid.NewGuid().ToString();
			notificationToDb.CreatedAt = DateTime.UtcNow;
			await _notificationRepository.CreateAsync(notificationToDb);
		}

		public async Task RemoveNotificationAsync(string notificationId)
		{
			var notificationFromDb = await _notificationRepository.GetAsync(n => n.Id == notificationId);

			if (notificationFromDb == null)
			{
				throw new Exception("Notification not found!");
			}

			await _notificationRepository.RemoveAsync(notificationFromDb);
		}
	}
}
