namespace NESTCOOKING_API.Business.DTOs.ReportDTOs
{
	public class CreateReportDTO
    {
        public string? TargetId { get; set; }
		public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ImagesURL { get; set; }
    }
}
