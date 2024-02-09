using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AdminDTOs;
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
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public ResponseService(IResponseRepository responseRepository, IMapper mapper, IReportRepository reportRepository)
        {
            _responseRepository = responseRepository;
            _mapper = mapper;
            _reportRepository = reportRepository;
        }
        public async Task<AdminResponseDTO> AdminHandleReportAsync(AdminRequestDTO adminRequestDTO)
        {
            var report = await _reportRepository.GetReportById(adminRequestDTO.ReportId);
            if (report == null)
            {
                throw new Exception(AppString.ReportNotFoundErrorMessage);
            };

            if (report.Status == StaticDetails.ActionStatus_ACCEPTED || report.Status == StaticDetails.ActionStatus_REJECTED)
            {
                throw new Exception(AppString.ReportAlreadyHandledErrorMessage);
            }
            var response = await _responseRepository.AdminHandleReportAsync(report, adminRequestDTO.AdminAction, adminRequestDTO.Title, adminRequestDTO.Content);
            AdminResponseDTO adminResponseDTO = _mapper.Map<AdminResponseDTO>(response);
            return adminResponseDTO;
        }
    }
}
