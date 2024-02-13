namespace NESTCOOKING_API.Business.DTOs.ReportDTOs
{
	public class UpdateReportDTO
    {
        public string Id { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Content { get; set; } = null!;
		public string? ImagesURL { get; set; }
    }
}
