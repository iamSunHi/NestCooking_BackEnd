using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs
{
	public class SearchResultDTO
	{
		public IEnumerable<UserShortInfoDTO> Users { get; set; }
		public IEnumerable<RecipeDTO> Recipes { get; set; }
	}
}
