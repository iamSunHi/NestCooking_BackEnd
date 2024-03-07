using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models.Admin;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class BookingStatisticRepository : Repository<BookingStatistic>, IBookingStatisticRepository
	{
		public BookingStatisticRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
