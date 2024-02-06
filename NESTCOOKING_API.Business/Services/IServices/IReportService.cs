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
        Task<ReportResponsDTO> CreateReportAsync(ReportDTO reportDto,string userId);
        Task<bool> DeleteReportAsync(string reportId);
        Task<ReportResponsDTO> UpdateReportAsync(string reportId, UpdateReportDTO updatedReportDto);
        Task<List<ReportResponsDTO>> GetAllReportsAsync();
        Task<List<ReportResponsDTO>> GetAllReportsByUserIdAsync(string userId);
     
    }
}
