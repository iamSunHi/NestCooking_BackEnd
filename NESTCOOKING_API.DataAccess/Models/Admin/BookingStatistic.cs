using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models.Admin
{
	public class BookingStatistic
	{
		[Key]
		public DateOnly Date { get; set; }
		public int TotalOfBooking { get; set; } = 0;
	}
}
