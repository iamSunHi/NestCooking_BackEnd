
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;

namespace NESTCOOKING_API.Business.DTOs.UserDTOs
{
    public class ChefDetailDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsMale { get; set; }
        public string Address { get; set; }
        public string AvatarUrl { get; set; }
        public int FollowerCount { get; set; }
        public IEnumerable<RecipeDTO> ListRecipes { get; set; }
    }
}
