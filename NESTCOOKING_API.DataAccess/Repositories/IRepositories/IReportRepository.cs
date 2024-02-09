using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<Report> AddReportAsync(User user, Report report);
        Task<bool> DeleteReportAsync(string reportId, string userId);
        Task<Report> UpdateReportAsync(string reportId, string Title, string content, string imagesURL,string userId);
        Task<List<Report>> GetAllReportsAsync();
        Task<List<Report>> GetAllReportsByUserIdAsync(string userId);

        Task<Report> GetReportById(string reportId);
    }
}
