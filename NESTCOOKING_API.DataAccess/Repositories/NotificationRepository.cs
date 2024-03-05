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
	}
}
