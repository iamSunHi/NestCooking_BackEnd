﻿using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface INotificationRepository : IRepository<Notification>
	{
		Task UpdateNotificationStatusAsync(string notificationId, string receiverId);
	}
}
