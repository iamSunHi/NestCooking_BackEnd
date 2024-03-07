using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Recipe
	{
		[Key]
		public string Id { get; set; } = null!;
		public string UserId { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
		public bool IsPrivate { get; set; }
		public double? RecipePrice { get; set; }
		public double Difficult { get; set; }
		public int CookingTime { get; set; }
		public int Portion { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public bool IsAvailableForBooking { get; set; } = false;
		public double? BookingPrice { get; set; }

		public ICollection<Comment> Comments { get; set; }
		public ICollection<BookingLine>	BookingLines { get; set; }
	}
}
