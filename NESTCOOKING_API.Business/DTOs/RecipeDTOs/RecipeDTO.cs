using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class RecipeDTO
	{
		public string Id { get; set; } = null!;
		public UserShortInfoDTO User { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
		public bool IsPrivate { get; set; }
		public double? RecipePrice { get; set; }
		public double Difficult { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public bool IsVerified { get; set; }
		public bool IsPublic { get; set; }
	}
}
