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
        Task<string> CreateReportAsync(ReportDTO reportDto,string userId);
        Task<bool> DeleteReportAsync(string reportId);
        Task<Report> UpdateReportAsync(string reportId, UpdateReportDTO updatedReportDto);
        Task<List<Report>> GetAllReportsAsync();
        Task<List<Report>> GetAllReportsByUserIdAsync(string userId);
     
    }
}
