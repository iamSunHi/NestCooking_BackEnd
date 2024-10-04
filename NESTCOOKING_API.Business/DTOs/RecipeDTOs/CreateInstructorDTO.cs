using FluentValidation;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
    public class CreateInstructorDTO
    {
        public string Description { get; set; }
        public IList<string>? ImageUrls { get; set; }
        public int InstructorOrder { get; set; }
    };
    public class CreateInstructorDTOValidator : AbstractValidator<CreateInstructorDTO>
    {
        public CreateInstructorDTOValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description Not null");
            RuleFor(x => x.InstructorOrder).GreaterThanOrEqualTo(0).WithMessage("InstructorOrder greater than 0");
        }
    }

}
