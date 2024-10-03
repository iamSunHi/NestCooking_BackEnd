using FluentValidation;

namespace NESTCOOKING_API.Business.DTOs.ReportDTOs
{
	public class UpdateReportDTO
    {
        public string Id { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Content { get; set; } = null!;
		public string? ImagesURL { get; set; }
    }
    public class UpdateReportDTOValidator : AbstractValidator<UpdateReportDTO>
    {
        public UpdateReportDTOValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.");

            RuleFor(x => x.ImagesURL)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().When(x => x.ImagesURL != null).WithMessage("ImagesURL is required if provided.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)).When(x => x.ImagesURL != null)
                .WithMessage("ImagesURL must be a valid URL.");
        }
    }
}
