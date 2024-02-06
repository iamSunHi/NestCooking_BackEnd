using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.ResponseDTOs;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IResponseService
    {
        public Task<AdminResponseDTO> AdminHandleReportAsync(string reportId,StaticDetails.AdminAction adminAction, string title, string content);
    }
}
