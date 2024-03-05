﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.Authentication;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AdminDTOs;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using NESTCOOKING_API.Business.DTOs.NotificationDTOs;
using NESTCOOKING_API.Business.DTOs.RecipeDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/admin")]
	[ApiController]
	[Authorize(StaticDetails.Role_Admin)]
	public class AdminController : ControllerBase
	{
		private PaginationInfoDTO _paginationInfo = new PaginationInfoDTO();
		private ICategoryService _categoryService;
		private readonly IResponseService _responseService;
		private readonly IReportService _reportService;
		private readonly ITransactionService _transactionService;
		private readonly INotificationService _notificationService;
		private readonly IRequestBecomeChefService _userRequestService;

		public AdminController(ICategoryService categoryService, IResponseService responseService, IReportService reportService, ITransactionService transactionService, INotificationService notificationService, IRequestBecomeChefService userRequestService)
		{
			_categoryService = categoryService;
			_responseService = responseService;
			_reportService = reportService;
			_transactionService = transactionService;
			_notificationService = notificationService;
			_userRequestService = userRequestService;
		}

		#region Category

		[HttpPost("categories/create")]
		public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryDTO categoryDTO)
		{
			try
			{
				var createdCategory = await _categoryService.CreateCategoryAsync(categoryDTO);
				if (createdCategory == null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: "This category already exists!"));
				}
				return Created();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPatch("categories/update")]
		public async Task<IActionResult> UpdateCategoryAsync([FromBody] CategoryDTO categoryDTO)
		{
			try
			{
				await _categoryService.UpdateCategoryAsync(categoryDTO);
				return Ok(ResponseDTO.Accept(result: categoryDTO));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("categories/delete/{categoryId}")]
		public async Task<IActionResult> DeleteCategoryAsync([FromRoute] int categoryId)
		{
			try
			{
				await _categoryService.DeleteCategoryAsync(categoryId);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		#endregion Category

		#region Report

		[HttpGet("reports")]
		public async Task<IActionResult> GetAllReports([FromQuery] string? status, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
		{
			try
			{
				var result = await _reportService.GetAllReportsAsync();

				if (status != null)
				{
					if (status.ToUpper() != StaticDetails.ActionStatus_ACCEPTED &&
						status.ToUpper() != StaticDetails.ActionStatus_COMPLETED &&
						status.ToUpper() != StaticDetails.ActionStatus_PENDING &&
						status.ToUpper() != StaticDetails.ActionStatus_REJECTED
						)
					{
						return BadRequest(ResponseDTO.BadRequest(message: "Invalid status!"));
					}
					result = result.Where(r => r.Status == status).ToList();
				}

				if (pageNumber != null || pageSize != null)
				{
					if (pageNumber == null || pageSize == null)
					{
						return BadRequest(ResponseDTO.BadRequest("You have to fill both pageNumber and pageSize if you want pagination."));
					}
					if (pageNumber == 0)
					{
						pageNumber = 1;
					}
					if (pageSize == 0)
					{
						pageSize = 1;
					}
					else if (pageSize > 100)
					{
						pageSize = 100;
					}
					result = result.Skip((int)((pageNumber - 1) * pageSize)).Take((int)pageSize).ToList();
				}

				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPost("reports")]
		public async Task<IActionResult> HandleReport([FromBody] AdminRequestDTO adminRequestDTO)
		{
			try
			{
				var result = await _responseService.AdminHandleReportAsync(adminRequestDTO);
				if (result != null)
				{
					return Ok(ResponseDTO.Accept(result: result));
				}
				else
				{
					return BadRequest(ResponseDTO.BadRequest());
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		#endregion Report

		#region Transaction

		[HttpGet("transactions")]
		public async Task<ActionResult> GetAllTransactions([FromQuery] string? type, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
		{
			try
			{
				var result = await _transactionService.GetAllTransactions();

				if (type != null)
				{
					if (type.ToUpper() != StaticDetails.PaymentType_DEPOSIT &&
						type.ToUpper() != StaticDetails.PaymentType_WITHDRAW
						)
					{
						return BadRequest(ResponseDTO.BadRequest(message: "Invalid type!"));
					}
					result = result.Where(r => r.Type == type).ToList();
				}

				if (pageNumber != null || pageSize != null)
				{
					if (pageNumber == null || pageSize == null)
					{
						return BadRequest(ResponseDTO.BadRequest("You have to fill both pageNumber and pageSize if you want pagination."));
					}
					if (pageNumber == 0)
					{
						pageNumber = 1;
					}
					if (pageSize == 0)
					{
						pageSize = 1;
					}
					else if (pageSize > 100)
					{
						pageSize = 100;
					}
					result = result.Skip((int)((pageNumber - 1) * pageSize)).Take((int)pageSize).ToList();
				}

				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		#endregion Transaction

		#region Notification

		[HttpGet("notifications")]
		public async Task<IActionResult> GetAllNotificationsWithPaginationAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
		{
			try
			{
				if (pageNumber != 0)
				{
					_paginationInfo.PageNumber = pageNumber;
				}
				if (pageSize != 0)
				{
					_paginationInfo.PageSize = pageSize;
				}
				else if (pageSize > 100)
				{
					_paginationInfo.PageSize = 100;
				}
				(int totalItems, int totalPages, IEnumerable<NotificationReadDTO> notificationList) result = await _notificationService.GetAllNotificationsWithPaginationAsync(_paginationInfo);

				if (result.notificationList == null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Page number is not valid!"));
				}
				return Ok(ResponseDTO.Accept(result: new
				{
					metadata = new
					{
						result.totalItems,
						result.totalPages,
						_paginationInfo.PageNumber,
						_paginationInfo.PageSize
					},
					notifications = result.notificationList
				}));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPost("notifications/create")]
		public async Task<IActionResult> CreateNotificationAsync([FromBody] NotificationCreateDTO notificationCreateDTO)
		{
			try
			{
				notificationCreateDTO.NotificationType = notificationCreateDTO.NotificationType.ToUpper();
				if (notificationCreateDTO.NotificationType != StaticDetails.NotificationType_RECIPE &&
					notificationCreateDTO.NotificationType != StaticDetails.NotificationType_REACTION &&
					notificationCreateDTO.NotificationType != StaticDetails.NotificationType_COMMENT &&
					notificationCreateDTO.NotificationType != StaticDetails.NotificationType_REPORT &&
					notificationCreateDTO.NotificationType != StaticDetails.NotificationType_REQUEST &&
					notificationCreateDTO.NotificationType != StaticDetails.NotificationType_RESPONSE)
				{
					return BadRequest(ResponseDTO.BadRequest(message: "Invalid notification type. Please try again."));
				}

				await _notificationService.CreateNotificationAsync(notificationCreateDTO);
				return Created();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpDelete("notifications/delete/{notificationId}")]
		public async Task<IActionResult> DeleteNotificationAsync([FromRoute] string notificationId)
		{
			try
			{
				await _notificationService.RemoveNotificationAsync(notificationId);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}

		#endregion Notification

		#region Request To Become Chef

		[HttpPut("requests/approval/{requestId}")]
		public async Task<IActionResult> ApprovalRequest(string requestId, [FromBody] ApprovalRequestDTO approvalRequestDTO)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				var Approvaled = await _userRequestService.ApprovalRequestByAdmin(requestId, userId, approvalRequestDTO);
				if (Approvaled != null)
				{
					return Ok(ResponseDTO.Accept(AppString.ApprovalRequestBecomeChefSuccessMessage, result: Approvaled));

				}
				else
				{
					return NotFound(AppString.RequestBecomeChefNotFound);
				}
			}
			catch (Exception ex)
			{

				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		#endregion Request To Become Chef
	}
}
