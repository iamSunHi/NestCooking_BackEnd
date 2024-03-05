using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AdminDTOs;
using NESTCOOKING_API.Business.DTOs.NotificationDTOs;
using NESTCOOKING_API.Business.DTOs.ResponseDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.Services
{
	public class ResponseService : IResponseService
	{
		private readonly UserManager<User> _userManager;
		private readonly IResponseRepository _responseRepository;
		private readonly IReportRepository _reportRepository;
		private readonly IRecipeRepository _recipeRepository;
		private readonly IUserRepository _userRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IRecipeService _recipeService;
		private readonly ICommentService _commentService;
		private readonly INotificationService _notificationService;
		private readonly IMapper _mapper;

		public ResponseService(UserManager<User> userManager,
			IResponseRepository responseRepository, IReportRepository reportRepository, IRecipeRepository recipeRepository, IUserRepository userRepository, ICommentRepository commentRepository,
			IRecipeService recipeService, ICommentService commentService, INotificationService notificationService,
			IMapper mapper)
		{
			_userManager = userManager;
			_responseRepository = responseRepository;
			_reportRepository = reportRepository;
			_recipeRepository = recipeRepository;
			_userRepository = userRepository;
			_commentRepository = commentRepository;
			_recipeService = recipeService;
			_commentService = commentService;
			_notificationService = notificationService;
			_mapper = mapper;
		}

		public async Task<AdminResponseDTO> AdminHandleReportAsync(AdminRequestDTO adminRequestDTO)
		{
			var report = await _reportRepository.GetAsync(r => r.Id == adminRequestDTO.ReportId);
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

			var notificationCreateDTO = new NotificationCreateDTO()
			{
				SenderId = null,
				ReceiverId = adminRequestDTO.ReportId,
				NotificationType = StaticDetails.NotificationType_RESPONSE
			};
			await _notificationService.CreateNotificationAsync(notificationCreateDTO);

			var violentUserId = string.Empty;
			switch (report.Type)
			{
				case StaticDetails.ReportType_RECIPE:
					{
						var recipe = await _recipeRepository.GetAsync(r => r.Id == report.TargetId);
						violentUserId = recipe.UserId;
						await _recipeService.DeleteRecipeAsync(violentUserId, recipe.Id);
						break;
					}
				case StaticDetails.ReportType_USER:
					{
						var user = await _userRepository.GetAsync(u => u.Id == report.TargetId);
						violentUserId = user.Id;
						await _userManager.SetLockoutEnabledAsync(user, true);
						await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(30));
						break;
					}
				case StaticDetails.ReportType_COMMENT:
					{
						var comment = await _commentRepository.GetAsync(c => c.CommentId == report.TargetId);
						violentUserId = comment.UserId;
						await _commentService.DeleteComment(violentUserId, report.TargetId);
						break;
					}
			}
			return adminResponseDTO;
		}
	}
}
