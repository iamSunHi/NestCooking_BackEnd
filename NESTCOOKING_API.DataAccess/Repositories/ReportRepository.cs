using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.DataAccess.Data;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.DataAccess.Repositories
{
	public class ReportRepository : Repository<Report>, IReportRepository
	{
		public ReportRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<Report> UpdateReportAsync(Report reportToDb, string userId)
		{
			try
			{
				var reportEntity = await _context.Reports
					.Where(r => r.Id == reportToDb.Id)
					.Include(r => r.User)
					.FirstOrDefaultAsync();

				if (reportEntity == null)
				{
					throw new Exception("Report not found!");
				}
				else
				{
					if (reportEntity.User.Id == userId || await _context.Roles.FirstOrDefaultAsync(r => r.Id == reportEntity.User.RoleId && r.Name == StaticDetails.Role_Admin) != null)
					{
						reportEntity.Title = reportToDb.Title;
						reportEntity.Content = reportToDb.Content;
						reportEntity.ImageUrls = reportToDb.ImageUrls;
						reportEntity.CreatedAt = DateTime.Now;

						await _context.SaveChangesAsync();
						return reportEntity;
					}
					else
					{
						throw new Exception("You don't have permission to update this.");
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> DeleteReportAsync(string reportId, string userId)
		{
			try
			{
				var reportEntity = await this.GetAsync(r => r.Id == reportId, includeProperties: "User");
				if (reportEntity == null)
				{
					throw new Exception("Report not found!");
				}
				else
				{
					if (reportEntity.User.Id == userId || await _context.Roles.FirstOrDefaultAsync(r => r.Id == reportEntity.User.RoleId && r.Name == StaticDetails.Role_Admin) != null)
					{
						await this.RemoveAsync(reportEntity);
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
				throw new Exception(ex.Message);
			}
		}
	}
}
