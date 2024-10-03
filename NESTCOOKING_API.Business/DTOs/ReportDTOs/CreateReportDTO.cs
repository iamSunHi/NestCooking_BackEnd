using FluentValidation;

namespace NESTCOOKING_API.Business.DTOs.ReportDTOs
{
	public class CreateReportDTO
    {
        public string? TargetId { get; set; }
		public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Content { get; set; } = null!;
        public List<string>? ImageUrls { get; set; }
    }
    public class CreateReportDTOValidator : AbstractValidator<CreateReportDTO>
    {
        public CreateReportDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.");

            RuleFor(x => x.ImageUrls)
                .Must(urls => urls == null || urls.Any())
                .WithMessage("ImageUrls cannot be an empty list.");
        }
    }
}
