using System.ComponentModel;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
    public class CreateIngredientDTO
    {
        public string Name { get; set; }
        public string Amount { get; set; }
        [DefaultValue(null)]
        public string? IngredientTipId { get; set; } = null;
    };

}
