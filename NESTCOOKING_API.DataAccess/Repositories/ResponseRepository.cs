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

namespace NESTCOOKING_API.DataAccess.Repositories
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly ApplicationDbContext _dbContext;


        public ResponseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Response> AdminHandleReportAsync(string reportId, StaticDetails.AdminAction adminAction,  string title, string content)
        {
            try
            {
                var report = await _dbContext.Reports
                    .Include(r => r.User)
                    .FirstAsync(r => r.Id == reportId);
                var reponseId = Guid.NewGuid().ToString();
                Response newResponse = new Response
                {
                    Id = reponseId,
                    User = report.User,
                    Title = title,
                    Content = content,
                    CreatedAt = DateTime.Now,
                };
                _dbContext.Responses.Add(newResponse);
                await _dbContext.SaveChangesAsync();
                report.Status = adminAction == StaticDetails.AdminAction.Accept ? StaticDetails.ActionStatus_ACCEPTED : StaticDetails.ActionStatus_REJECTED;
                report.Response = newResponse;
                await _dbContext.SaveChangesAsync();
                return newResponse;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
