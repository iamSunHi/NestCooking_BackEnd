using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.Authentication;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AdminDTOs;
using NESTCOOKING_API.Business.DTOs.ChefRequestDTOs;
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
		private readonly IStatisticService _statisticService;
		private readonly IRequestBecomeChefService _requestBecomeChefService;
		private readonly IRecipeService _recipeService;
		public AdminController(IStatisticService statisticService, ICategoryService categoryService, IResponseService responseService,
			IReportService reportService, ITransactionService transactionService, INotificationService notificationService,
			IRequestBecomeChefService requestBecomeChef, IRecipeService recipeService)
		{
			_categoryService = categoryService;
			_responseService = responseService;
			_reportService = reportService;
			_transactionService = transactionService;
			_notificationService = notificationService;
			_statisticService = statisticService;
			_requestBecomeChefService = requestBecomeChef;
			_recipeService = recipeService;
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
					notificationCreateDTO.NotificationType != StaticDetails.NotificationType_RESPONSE &&
					notificationCreateDTO.NotificationType != StaticDetails.NotificationType_ANNOUNCEMENT
					)
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

		#region RequestBecomeChef


		[HttpGet("request")]
		public async Task<IActionResult> GetAllRequests([FromQuery] string? status, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
		{
			try
			{
				var result = await _requestBecomeChefService.GetAllRequestsToBecomeChef();
				if (status != null)
				{
					status = status.ToUpper();
					if (status != StaticDetails.ActionStatus_ACCEPTED &&
						status != StaticDetails.ActionStatus_COMPLETED &&
						status != StaticDetails.ActionStatus_PENDING &&
						status != StaticDetails.ActionStatus_REJECTED
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

		[HttpPut("request/verify")]
		public async Task<IActionResult> ApprovalRequest([FromBody] ApprovalRequestDTO approvalRequestDTO)
		{
			try
			{
				var result = await _requestBecomeChefService.ApprovalRequestByAdmin(approvalRequestDTO);
				return result != null ? Ok(ResponseDTO.Accept(result: result))
				   : NotFound(AppString.RequestBecomeChefNotFound);
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		#endregion RequestBecomeChef

		#region Statistic

		[HttpGet("statistics")]
		public async Task<IActionResult> GetAllStatisticsAsync()
		{
			try
			{
				var result = await _statisticService.GetAllStatisticsAsync();
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					return BadRequest(ResponseDTO.BadRequest(message: ex.InnerException.Message));
				}
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		#endregion Statistic

		#region Verify recipes

		[HttpGet("recipe/unverified")]
		[ResponseCache(Duration = 30)]
		public async Task<IActionResult> GetAllUnverifiedRecipesAsync([FromQuery] string? id = null)
		{
			try
			{
				if (string.IsNullOrEmpty(id))
				{
					var recipes = await _recipeService.GetAllUnverifiedRecipesAsync();
					recipes = recipes.OrderByDescending(r => r.CreatedAt).ToList();
					return Ok(ResponseDTO.Accept(result: recipes));
				}
				else
				{
					var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
					var recipe = await _recipeService.GetRecipeByIdAsync(id, userId);
					if (recipe == null)
					{
						return BadRequest(ResponseDTO.BadRequest(message: $"Recipe with id {id} not found."));
					}
					return Ok(ResponseDTO.Accept(result: recipe));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPut("recipe/verify")]
		public async Task<IActionResult> GetAllUnverifiedRecipesAsync([FromBody] AdminVerifyRecipeDTO verifyRecipeDTO)
		{
			try
			{
				verifyRecipeDTO.Status = verifyRecipeDTO.Status.ToUpper();
				await _recipeService.VerifyRecipe(verifyRecipeDTO);
				return Ok(ResponseDTO.Accept());
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		#endregion Verify recipes
	}
}
