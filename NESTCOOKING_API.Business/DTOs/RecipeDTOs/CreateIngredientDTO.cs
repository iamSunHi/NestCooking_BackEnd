using FluentValidation;
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
    public class CreateIngredientDTOValidator : AbstractValidator<CreateIngredientDTO>
    {
        public CreateIngredientDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name Not null");
            RuleFor(x => x.Amount).NotEmpty().WithMessage("Amount not null");
        }
    }

}
