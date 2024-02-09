using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IReportService
    {
        Task<ReportResponseDTO> CreateReportAsync(CreateReportDTO reportDto,string userId);
        Task<bool> DeleteReportAsync(string reportId, string userId);
        Task<ReportResponseDTO> UpdateReportAsync(string reportId, UpdateReportDTO updatedReportDto,string userId);
        Task<List<ReportResponseDTO>> GetAllReportsAsync();
        Task<List<ReportResponseDTO>> GetAllReportsByUserIdAsync(string userId);
     
    }
}
