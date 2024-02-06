using AutoMapper;
using AutoMapper.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Migrations;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;
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
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;
        public ReportService(IReportRepository reportRepository, IMapper mapper, IUserRepository userRepository)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<ReportResponseDTO> CreateReportAsync(CreateReportDTO createReportDto, string userId)
        {
            var user = await _userRepository.GetAsync(u => u.Id == userId);
            var createdReportMapped = _mapper.Map<Report>(createReportDto);
            if (createReportDto.Type == ReportType_USER)
            {
                var targetUser = await _userRepository.GetAsync(u => u.Id == createReportDto.TargetId);
                if (targetUser == null) {
                    throw new UserNotFoundException();
                }
                createdReportMapped.TargetId = targetUser.Id;
            }
            // if (createReportDto.Type == ReportType.comment.ToString())
            // {
            //     var comment = await _reportRepository.GetCommentByIdAsync(createReportDto.TargetId);
            //     createdReportMapped.Comment = comment;
            // }
            // else if (createReportDto.Type == ReportType.recipe.ToString())
            // {
            //     var recipe = await _reportRepository.GetRecipeByIdAsync(createReportDto.TargetId);
            //     createdReportMapped.Recipe = recipe;
            // }
            var report = await _reportRepository.AddReportAsync(user, createdReportMapped);
            var reportResponse = _mapper.Map<ReportResponseDTO>(report);
            return reportResponse;
        }

        public async Task<bool> DeleteReportAsync(string reportId)
        {
            return await _reportRepository.DeleteReportAsync(reportId);
        }
        public async Task<ReportResponseDTO> UpdateReportAsync(string reportId, UpdateReportDTO updatedReportDto)
        {
            var report = await _reportRepository.UpdateReportAsync(reportId, updatedReportDto.Title, updatedReportDto.Content, updatedReportDto.ImagesURL);
            ReportResponseDTO reportResponeDTO = new ReportResponseDTO();
            reportResponeDTO = _mapper.Map<ReportResponseDTO>(report);
            return reportResponeDTO;
        }
        public async Task<List<ReportResponseDTO>> GetAllReportsAsync()
        {
            try
            {
                var reports = await _reportRepository.GetAllReportsAsync();
                var reportDTOs = reports.Select(report => _mapper.Map<ReportResponseDTO>(report)).ToList();

                return reportDTOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<ReportResponseDTO>> GetAllReportsByUserIdAsync(string userId)
        {
            try
            {
                var reports = await _reportRepository.GetAllReportsByUserIdAsync(userId);
                var reportDTOs = reports.Select(report => _mapper.Map<ReportResponseDTO>(report)).ToList();

                return reportDTOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
