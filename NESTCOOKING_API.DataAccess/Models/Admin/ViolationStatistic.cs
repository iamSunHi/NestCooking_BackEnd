using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models.Admin
{
	public class ViolationStatistic
	{
		[Key]
		public DateOnly Date { get; set; }
		public int TotalOfViolation { get; set; } = 0;
	}
}
