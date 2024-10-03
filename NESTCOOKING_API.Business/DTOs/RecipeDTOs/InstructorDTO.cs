using FluentValidation;

namespace NESTCOOKING_API.Business.DTOs.RecipeDTOs
{
	public class InstructorDTO
	{
		public int Id { get; set; }
		public string Description { get; set; } = null!;
		public List<string>? ImageUrls { get; set; }
		public int InstructorOrder { get; set; }
	}
    public class InstructorDTOValidator : AbstractValidator<InstructorDTO>
    {
        public InstructorDTOValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description Not null");
            RuleFor(x => x.InstructorOrder).GreaterThanOrEqualTo(0).WithMessage("InstructorOrder greater than 0");
        }
    }
}
