﻿using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class NotificationRepository : Repository<Notification>, INotificationRepository
	{
		public NotificationRepository(ApplicationDbContext context) : base(context)
		{

		}

		public async Task UpdateAsync(Notification notification)
		{
			// Assuming that the entity is already attached to the context
			_context.Entry(notification).State = EntityState.Modified;

			// Alternatively, if the entity is not attached, you can use:
			// _context.Notifications.Update(notification);

			await _context.SaveChangesAsync();
		}

		public async Task UpdateNotificationStatusAsync(string notificationId, string receiverId)
		{
			var notificationFromDb = await this.GetAsync(n => n.Id == notificationId && n.ReceiverId == receiverId);

			if (notificationFromDb != null)
			{
				if (_context.Entry(notificationFromDb).State == EntityState.Detached)
				{
					_context.Attach(notificationFromDb);
				}

				notificationFromDb.IsSeen = true;
				await _context.SaveChangesAsync();
			}
			else
			{
				throw new Exception(AppString.SomethingWrongMessage);
			}
		}
	}
}
