using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class RecipeDTO
	{
		public string Id { get; set; } = null!;
		public UserShortInfoDTO Owner { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
	}
}
