using AutoMapper;
using AutoMapper.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using NESTCOOKING_API.Business.Services.IServices;
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
        private readonly Mapper mapper;
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<string> CreateReportAsync(ReportDTO reportDto,string userId)
        {
               
            return  await _reportRepository.AddReportAsync(userId, reportDto.TargetId, reportDto.Title,reportDto.Content, reportDto.ImagesURL);
        }

        public async Task<bool> DeleteReportAsync(string reportId)
        {
            return await _reportRepository.DeleteReportAsync(reportId);
        }
        public async Task<Report> UpdateReportAsync(string reportId, UpdateReportDTO updatedReportDto)
        {

            return await _reportRepository.UpdateReportAsync(reportId, updatedReportDto.Title,updatedReportDto.Content,updatedReportDto.ImagesURL);
        }
        public async Task<List<Report>> GetAllReportsAsync()
        {
   
            return await _reportRepository.GetAllReportsAsync();
        }
        public async Task<List<Report>> GetAllReportsByUserIdAsync(string userId)
        {

            return await _reportRepository.GetAllReportsByUserIdAsync(userId);
        }
     
    }
}
