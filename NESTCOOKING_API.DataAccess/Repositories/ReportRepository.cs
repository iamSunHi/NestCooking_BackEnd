using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.DataAccess.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _dbContext;
 

        public ReportRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> AddReportAsync(string userId, string targetId, string title, string content,string imagesURL)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            var target = await _dbContext.Users.FindAsync(targetId);

            var reportEntity = new Report
            {
                Id = Guid.NewGuid().ToString(),
                User = user,
                Target = target,
                Title = title,
                Content = content,
                ImageUrl = imagesURL,
                Status = StaticDetails.ActionStatus.PENDING.ToString(),
                CreatedAt = DateTime.Now,
                Response = null
            };
            try
                {
                    await _dbContext.AddAsync(reportEntity);
                    await _dbContext.SaveChangesAsync();
                    return reportEntity.Id;
                }
                catch (Exception ex)
                {
                    return null;
                }
        }
        public async Task<bool> DeleteReportAsync(string reportId)
        {
            try
            {
                var reportEntity = await _dbContext.Reports.FindAsync(reportId);

                if (reportEntity == null)
                {
                    return false;
                }

                _dbContext.Reports.Remove(reportEntity);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }
        public async Task<Report> UpdateReportAsync(string reportId, string title, string content, string imagesURL)
        {
            try
            {
                var reportEntity = await _dbContext.Reports
                .Include(r => r.User)
                .Include(r => r.Target)
                .FirstOrDefaultAsync(r => r.Id == reportId);

                if (reportEntity == null)
                {
                    return null;
                }

             
                reportEntity.Title = title;
                reportEntity.Content = content;
                reportEntity.ImageUrl = imagesURL;
                reportEntity.CreatedAt= DateTime.Now;


                // Optionally, update other properties as needed

                await _dbContext.SaveChangesAsync();

                return reportEntity;


            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<Report>> GetAllReportsAsync()
        {
            // Assuming you have a suitable mapping method to convert Report entities to ReportDTO
            var reports = await _dbContext.Reports
          .Include(r => r.User)
          .Include(r => r.Target)
          .ToListAsync();

            return reports;
        }
        public async Task<List<Report>> GetAllReportsByUserIdAsync(string userId)
        {
            // Assuming you have a suitable mapping method to convert Report entities to ReportDTO
            var reports = await _dbContext.Reports
                .Include(r => r.User)
                .Include(r => r.Target)
                .Where(report => report.User.Id == userId)
                .ToListAsync();

            return reports.ToList();
        }

    }
}
