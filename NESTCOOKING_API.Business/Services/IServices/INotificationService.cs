using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.NotificationDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface INotificationService
	{
		Task<(int, int, IEnumerable<NotificationReadDTO>)> GetAllNotificationsWithPaginationAsync(PaginationInfoDTO paginationInfo);
		Task<IEnumerable<NotificationReadDTO>> GetAllNotificationsByReceiverIdAsync(string receiverId);
		Task CreateNotificationAsync(NotificationCreateDTO notificationCreateDTO);
		Task<bool> SeenAllUserNotificationsAsync(string userId);
		Task UpdateNotificationStatusByIdAsync(string notificationId, string receiverId);
		Task UpdateAllNotificationStatusAsync(string receiverId);
		Task RemoveNotificationAsync(string notificationId);
	}
}
