using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IReportRepository
    {
       Task<string> AddReportAsync(string userId, string targetId, string Title,string content, string imagesURL);
       Task<bool> DeleteReportAsync(string reportId);
       Task<Report> UpdateReportAsync(string reportId, string Title, string content, string imagesURL);
        Task<List<Report>> GetAllReportsAsync();
        Task<List<Report>> GetAllReportsByUserIdAsync(string userId);

    }
}
