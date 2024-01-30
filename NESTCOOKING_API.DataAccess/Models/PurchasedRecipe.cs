using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class PurchasedRecipe
	{
		[Key]
		public int Id { get; set; }
		public User User { get; set; }
		public Recipe Recipe { get; set; }
	}
}
