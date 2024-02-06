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
        private readonly IMapper _mapper;

        public ResponseService(IResponseRepository responseRepository,IMapper mapper)
        {
            _responseRepository = responseRepository;
            _mapper = mapper;
        }
        public async Task<AdminResponseDTO> AdminHandleReportAsync(string reportId, StaticDetails.AdminAction adminAction, string title , string content)
        {

            var response = await _responseRepository.AdminHandleReportAsync(reportId, adminAction,title,content);
            AdminResponseDTO adminResponseDTO = new AdminResponseDTO();
            adminResponseDTO = _mapper.Map<AdminResponseDTO>(response);
            return adminResponseDTO;
        }
    }
}
