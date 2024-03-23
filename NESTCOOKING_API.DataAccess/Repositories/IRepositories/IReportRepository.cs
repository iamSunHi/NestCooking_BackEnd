using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
	public interface IReportRepository : IRepository<Report>
    {
        Task<bool> DeleteReportAsync(string reportId, string userId);
        Task<Report> UpdateReportAsync(Report reportToDb,string userId);
    }
}
