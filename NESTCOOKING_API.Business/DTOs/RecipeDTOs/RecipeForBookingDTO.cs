namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class RecipeForBookingDTO
	{
		public string Id { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
		public int Portion { get; set; }
		public int BookingPrice { get; set; }
	}
}
