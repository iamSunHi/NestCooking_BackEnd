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
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ReportRepository(ApplicationDbContext dbContext, UserManager<User> userManager) : base(dbContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<Report> AddReportAsync(User user, Report report)
        {
            //if (await _dbContext.Users.AnyAsync(u => u.Id == targetId))
            //{
            //     reportType = StaticDetails.ReportType.user.ToString();
            //}
            //else if (await _dbContext.Comments.AnyAsync(c => c.Id == targetId))
            //{
            //    reportType = StaticDetails.ReportType.comment.ToString();
            //}
            //else if (await _dbContext.Recipes.AnyAsync(r => r.Id == targetId))
            //{

            //    reportType = StaticDetails.ReportType.recipe.ToString();
            //}
            var reportEntity = new Report
            {
                Id = Guid.NewGuid().ToString(),
                User = user,
                TargetId = report.TargetId,
                Title = report.Title,
                Type = report.Type,
                Content = report.Content,
                ImageUrl = report.ImageUrl,
                Status = StaticDetails.ActionStatus_PENDING,
                CreatedAt = DateTime.Now,
                Response = null
            };
            try
            {
                await _dbContext.AddAsync(reportEntity);
                await _dbContext.SaveChangesAsync();
                return reportEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> DeleteReportAsync(string reportId, string userId)
        {
            try
            {
                var reportEntity = await _dbContext.Reports.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == reportId);
                if (reportEntity == null)
                {
                    return false;
                }
                else
                {
                    if (reportEntity.User.Id == userId || await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == reportEntity.User.RoleId && r.Name == "admin")!= null)
                    {


                        await RemoveAsync(reportEntity);
                        await _dbContext.SaveChangesAsync();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<Report> UpdateReportAsync(string reportId, string title, string content, string imagesURL, string userId)
        {
            try
            {
                var reportEntity = await _dbContext.Reports
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == reportId);
                if (reportEntity == null)
                {
                    return null;
                }
                else
                {
                    if (reportEntity.User.Id == userId || await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == reportEntity.User.RoleId && r.Name == "admin") != null)
                    {
                        reportEntity.Title = title;
                        reportEntity.Content = content;
                        reportEntity.ImageUrl = imagesURL;
                        reportEntity.CreatedAt = DateTime.Now;
                        await _dbContext.SaveChangesAsync();
                        return reportEntity;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<Report>> GetAllReportsAsync()
        {
            var reports = await _dbContext.Reports
          .Include(r => r.User)
          .ToListAsync();

            return reports;
        }
        public async Task<List<Report>> GetAllReportsByUserIdAsync(string userId)
        {
            var reports = await _dbContext.Reports
                .Include(r => r.User)
                .Where(report => report.User.Id == userId)
                .ToListAsync();

            return reports.ToList();
        }
        public async Task<Report> GetReportById(string reportId)
        {
            return await _dbContext.Reports.Where(r => r.Id == reportId)
            .Include(r => r.User)
            .FirstOrDefaultAsync();
        }
    }
}
