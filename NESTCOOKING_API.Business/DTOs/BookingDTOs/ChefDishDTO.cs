namespace NESTCOOKING_API.Business.DTOs.BookingDTOs
{
	public class ChefDishDTO
	{
		public string? Id { get; set; }
		public string? ChefId { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public double Price { get; set; }
		public int Portion { get; set; } = 1;
		public string? RecipeUrl { get; set; }
		public List<string>? ImageUrls { get; set; }
	}
}
