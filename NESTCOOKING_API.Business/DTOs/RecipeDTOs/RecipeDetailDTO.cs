using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using NESTCOOKING_API.Business.DTOs.ReactionDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class RecipeDetailDTO
	{
		public string Id { get; set; } = null!;
		public UserShortInfoDTO User { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? ThumbnailUrl { get; set; }
		public bool IsPrivate { get; set; }
		public double? RecipePrice { get; set; }
		public double Difficult { get; set; }
		public int CookingTime { get; set; }
		public int Portion { get; set; }
		public IEnumerable<CategoryDTO> Categories { get; set; }
		public IEnumerable<IngredientDTO> Ingredients { get; set; }
		public IEnumerable<InstructorDTO> Instructors { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public Dictionary<string, List<string>> Reactions { get; set; }
		public IEnumerable<RequestCommentDTO> Comments { get; set; }
	}
}
