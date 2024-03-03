using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AdminDTOs;
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
		private ICategoryService _categoryService;
		private readonly IResponseService _responseService;
		private readonly IReportService _reportService;
		private readonly ITransactionService _transactionService;

		public AdminController(ICategoryService categoryService, IResponseService responseService, IReportService reportService, ITransactionService transactionService)
		{
			_categoryService = categoryService;
			_responseService = responseService;
			_reportService = reportService;
			_transactionService = transactionService;
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
	}
}
