namespace NESTCOOKING_API.DataAccess.Models
{
	public class ChefDish
	{
		public string Id { get; set; } = null!;
		public string ChefId { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public double Price { get; set; }
		public int Portion { get; set; } = 1;
		public string? RecipeUrl { get; set; }
		public List<string>? ImageUrls { get; set; }
	}
}