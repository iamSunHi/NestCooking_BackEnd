using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models.Admin
{
	public class UserStatistic
	{
		[Key]
		public DateOnly Date { get; set; }
		public int NumberOfNewUser { get; set; } = 0;
		public int TotalOfUser { get; set; } = 0;
	}
}
