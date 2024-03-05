namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class RecipeForBookingDTO
	{
		public string Id { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public int Portion { get; set; }
		public string? ThumbnailUrl { get; set; }

		public bool IsAvailableForBooking { get; set; }
		public double? BookingPrice { get; set; }
	}
}
