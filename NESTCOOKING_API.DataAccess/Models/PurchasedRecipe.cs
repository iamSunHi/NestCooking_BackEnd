using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class PurchasedRecipe
	{
		[Key]
		public string Id { get; set; }
		public string UserId { get; set; }
        public User User { get; set; }
        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}
