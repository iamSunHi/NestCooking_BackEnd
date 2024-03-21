using AutoMapper;
using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

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

		public async Task<List<ReportResponseDTO>> GetAllReportsAsync()
		{
			try
			{
				var reportsFromDb = await _reportRepository.GetAllAsync(includeProperties: "User");
				var reportDTOs = _mapper.Map<List<ReportResponseDTO>>(reportsFromDb);
				return reportDTOs;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public async Task<List<ReportResponseDTO>> GetAllReportsByUserIdAsync(string userId)
		{
			try
			{
				var reportsFromDb = await _reportRepository.GetAllAsync(r => r.UserId == userId, includeProperties: "User");
				var reportDTOs = _mapper.Map<List<ReportResponseDTO>>(reportsFromDb);
				return reportDTOs;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public async Task CreateReportAsync(CreateReportDTO createReportDTO, string userId)
		{
			var user = await _userRepository.GetAsync(u => u.Id == userId);
			var createdReportMapped = _mapper.Map<Report>(createReportDTO);
			if (createReportDTO.Type == StaticDetails.ReportType_USER)
			{
				var targetUser = await _userRepository.GetAsync(u => u.Id == createReportDTO.TargetId);
				if (targetUser == null)
				{
					throw new UserNotFoundException();
				}
				createdReportMapped.TargetId = targetUser.Id;
			}
			createdReportMapped.Id = Guid.NewGuid().ToString();
			createdReportMapped.User = user;
			createdReportMapped.CreatedAt = DateTime.UtcNow.AddHours(7);
			createdReportMapped.Status = StaticDetails.ActionStatus_PENDING;

			await _reportRepository.CreateAsync(createdReportMapped);
		}

		public async Task<ReportResponseDTO> UpdateReportAsync(UpdateReportDTO updatedReportDTO, string userId)
		{
			var reportToDb = _mapper.Map<Report>(updatedReportDTO);
			var report = await _reportRepository.UpdateReportAsync(reportToDb, userId);

			var reportResponseDTO = _mapper.Map<ReportResponseDTO>(report);
			return reportResponseDTO;
		}

		public async Task<bool> DeleteReportAsync(string reportId, string userId)
		{
			return await _reportRepository.DeleteReportAsync(reportId, userId);
		}
	}
}
