using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models.Admin
{
	public class ChefStatistic
	{
		[Key]
		public DateOnly Date { get; set; }
		public int NumberOfNewChef { get; set; } = 0;
		public int TotalOfChef { get; set; } = 0;
	}
}
