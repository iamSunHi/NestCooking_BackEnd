using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.ResponseDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.Services
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseRepository _responseRepository;
        public ResponseService(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
        }
        public async Task<AdminResponseDTO> AdminHandleReportAsync(string reportId, StaticDetails.AdminAction adminAction, AdminResponseDTO responseDTO)
        {
            var userId = responseDTO.UserId;
            var title = responseDTO.Title;
            var content = responseDTO.Content;

            var response = await _responseRepository.AdminHandleReportAsync(reportId, adminAction,userId,title,content);
            AdminResponseDTO adminResponseDTO = new AdminResponseDTO();
            adminResponseDTO.UserId = response.User.Id;
            adminResponseDTO.Title= title;
            adminResponseDTO.Content = content;
            return adminResponseDTO;
        }
    }
}
