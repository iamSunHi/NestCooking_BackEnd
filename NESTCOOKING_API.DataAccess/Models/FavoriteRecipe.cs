namespace NESTCOOKING_API.DataAccess.Models
{
	public class FavoriteRecipe
	{
		public string UserId { get; set; } = null!;
		public User User { get; set; }

		public string RecipeId { get; set; } = null!;
		public Recipe Recipe { get; set; }
	}
}
