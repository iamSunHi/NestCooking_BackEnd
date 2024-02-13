using NESTCOOKING_API.Business.DTOs.ReportDTOs;

namespace NESTCOOKING_API.Business.Services.IServices
{
	public interface IReportService
	{
		Task<List<ReportResponseDTO>> GetAllReportsAsync();
		Task<List<ReportResponseDTO>> GetAllReportsByUserIdAsync(string userId);
		Task CreateReportAsync(CreateReportDTO createReportDTO, string userId);
		Task<ReportResponseDTO> UpdateReportAsync(UpdateReportDTO updatedReportDto, string userId);
		Task<bool> DeleteReportAsync(string reportId, string userId);
    }
}
