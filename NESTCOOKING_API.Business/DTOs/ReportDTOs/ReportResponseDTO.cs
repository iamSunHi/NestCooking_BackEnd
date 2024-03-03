using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.ReportDTOs
{
    public class ReportResponseDTO
    {
        public string Id { get; set; } = null!;
        public UserDTO User { get; set; }
        public string TargetId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Content { get; set; } = null!;
        public List<string>? ImageUrls { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
