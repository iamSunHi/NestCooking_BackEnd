using AutoMapper;
using AutoMapper.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Migrations;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Business.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        public ReportService(IReportRepository reportRepository,IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task<ReportResponsDTO> CreateReportAsync(ReportDTO reportDto,string userId)
        {
            var report = await _reportRepository.AddReportAsync(userId, reportDto.TargetId, reportDto.Title, reportDto.Content, reportDto.ImagesURL);
            var reportResponse = _mapper.Map<ReportResponsDTO>(report);
            return reportResponse;
        }

        public async Task<bool> DeleteReportAsync(string reportId)
        {
            return await _reportRepository.DeleteReportAsync(reportId);
        }
        public async Task<ReportResponsDTO> UpdateReportAsync(string reportId, UpdateReportDTO updatedReportDto)
        {
            var report = await _reportRepository.UpdateReportAsync(reportId, updatedReportDto.Title, updatedReportDto.Content, updatedReportDto.ImagesURL);
            ReportResponsDTO reportResponeDTO = new ReportResponsDTO();
            reportResponeDTO =  _mapper.Map<ReportResponsDTO>(report);
            return reportResponeDTO;
        }
        public async Task<List<ReportResponsDTO>> GetAllReportsAsync()
        {
            try
            {
                var reports = await _reportRepository.GetAllReportsAsync();
                var reportDTOs = reports.Select(report => _mapper.Map<ReportResponsDTO>(report)).ToList();

                return reportDTOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<ReportResponsDTO>> GetAllReportsByUserIdAsync(string userId)
        {
            try
            {
                var reports = await _reportRepository.GetAllReportsByUserIdAsync(userId);
                var reportDTOs = reports.Select(report => _mapper.Map<ReportResponsDTO>(report)).ToList();

                return reportDTOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
     
    }
}
