using NESTCOOKING_API.DataAccess.Migrations;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.DataAccess.Repositories.IRepositories
{
    public interface IResponseRepository
    {
        public Task<Response> AdminHandleReportAsync(string reportId, StaticDetails.AdminAction adminAction, string title, string content);
    }
}
