using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models.Admin
{
	public class RevenueStatistic
	{
		[Key]
		public DateOnly Date { get; set; }
		public double Revenue { get; set; } = 0.0;
	}
}
