using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.NotificationDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.Services
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationRepository _notificationRepository;
		private readonly IUserRepository _userRepository;
		private readonly IRecipeRepository _recipeRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IReportRepository _reportRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly IMapper _mapper;

		public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository,
			IRecipeRepository recipeRepository, ICommentRepository commentRepository, IReportRepository reportRepository,
			IMapper mapper, IRoleRepository roleRepository)
		{
			_notificationRepository = notificationRepository;
			_userRepository = userRepository;
			_recipeRepository = recipeRepository;
			_commentRepository = commentRepository;
			_reportRepository = reportRepository;
			_mapper = mapper;
			_roleRepository = roleRepository;
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

			switch (notificationCreateDTO.NotificationType)
			{
				case StaticDetails.NotificationType_REACTION:
					{
						switch (notificationCreateDTO.TargetType)
						{
							case StaticDetails.TargetType_RECIPE:
								{
									var target = await _recipeRepository.GetAsync(r => r.Id == notificationToDb.ReceiverId);
									notificationToDb.ReceiverId = target.UserId;
									break;
								}
							case StaticDetails.TargetType_COMMENT:
								{
									var target = (await _commentRepository.GetAsync(r => r.CommentId == notificationToDb.ReceiverId));
									notificationToDb.ReceiverId = target.UserId;
									break;
								}
						}

						if (notificationToDb.SenderId == notificationToDb.ReceiverId)
							return;

						var sender = await _userRepository.GetAsync(u => u.Id == notificationToDb.SenderId);
						notificationToDb.Content = sender.FirstName + " " + sender.LastName + AppString.NotificationReaction + notificationCreateDTO.TargetType + ".";
						await _notificationRepository.CreateAsync(notificationToDb);

						break;
					}
				case StaticDetails.NotificationType_COMMENT:
					{
						var target = await _commentRepository.GetAsync(r => r.CommentId == notificationToDb.ReceiverId);
						notificationToDb.ReceiverId = target.UserId;

						if (notificationToDb.SenderId == notificationToDb.ReceiverId)
							return;

						var sender = await _userRepository.GetAsync(u => u.Id == notificationToDb.SenderId);
						if (notificationCreateDTO.TargetType == StaticDetails.CommentType_RECIPE)
						{
							notificationToDb.Content = sender.FirstName + " " + sender.LastName + AppString.NotificationCommentInRecipe;
						}
						else
						{
							notificationToDb.Content = sender.FirstName + " " + sender.LastName + AppString.NotificationCommentReply;
						}
						await _notificationRepository.CreateAsync(notificationToDb);

						break;
					}
				case StaticDetails.NotificationType_RESPONSE:
					{
						var target = await _reportRepository.GetAsync(r => r.Id == notificationCreateDTO.ReceiverId);

						if (target.Status == StaticDetails.ActionStatus_ACCEPTED)
						{
							// Send notification for violent user
							switch (target.Type)
							{
								case StaticDetails.ReportType_USER:
									{
										notificationToDb.ReceiverId = target.TargetId;
										break;
									}
								case StaticDetails.ReportType_RECIPE:
									{
										var recipeFromDb = await _recipeRepository.GetAsync(r => r.Id == target.TargetId);
										notificationToDb.ReceiverId = recipeFromDb.UserId;
										break;
									}
								case StaticDetails.ReportType_COMMENT:
									{
										var commentFromDb = await _commentRepository.GetAsync(c => c.CommentId == target.TargetId);
										notificationToDb.ReceiverId = commentFromDb.UserId;
										break;
									}
							}
							notificationToDb.Content = AppString.NotificationApproveReportForViolentUser;
							await _notificationRepository.CreateAsync(notificationToDb);

							// Send notification for reporter
							notificationToDb.ReceiverId = target.UserId;
							notificationToDb.Id = Guid.NewGuid().ToString();
							notificationToDb.ReceiverId = target.UserId;
							notificationToDb.Content = AppString.NotificationApproveReport;
						}
						else
						{
							notificationToDb.ReceiverId = target.UserId;
							notificationToDb.Content = AppString.NotificationRejectReport;
						}
						await _notificationRepository.CreateAsync(notificationToDb);

						break;
					}
				case StaticDetails.NotificationType_ANNOUNCEMENT:
					{
						var roleAdmin = await _roleRepository.GetRoleIdByNameAsync(StaticDetails.Role_Admin);
						var listAdmin = await _userRepository.GetAllAsync(u => u.RoleId == roleAdmin);
						var listAllUserInSystem = await _userRepository.GetAllAsync();

						foreach (var user in listAllUserInSystem)
						{
							if (user.RoleId == roleAdmin)
							{
								continue;
							}
							notificationToDb.Id = Guid.NewGuid().ToString();
							notificationToDb.ReceiverId = user.Id;
							notificationToDb.Content = notificationCreateDTO.Content;
							await _notificationRepository.CreateAsync(notificationToDb);
						}
						break;
					}
			}

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

		public async Task<bool> SeenAllUserNotificationsAsync(string userId)
		{
			var listUserNotification = await _notificationRepository.GetAllAsync(n => n.ReceiverId == userId);
			foreach (var notification in listUserNotification)
			{
				notification.IsSeen = true;
				await _notificationRepository.UpdateAsync(notification);
			}

			return true;
		}

		public async Task UpdateNotificationStatusByIdAsync(string notificationId, string receiverId)
		{
			await _notificationRepository.UpdateNotificationStatusAsync(notificationId, receiverId);
		}

		public async Task UpdateAllNotificationStatusAsync(string receiverId)
		{
			var unseenNotificationListFromDb = await _notificationRepository.GetAllAsync(n => n.ReceiverId == receiverId);

			foreach (var notification in unseenNotificationListFromDb)
			{
				await _notificationRepository.UpdateNotificationStatusAsync(notification.Id, receiverId);
			}
		}
	}
}
