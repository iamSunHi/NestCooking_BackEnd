using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

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
	}
}
