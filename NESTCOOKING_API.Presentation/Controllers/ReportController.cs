using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Security.Claims;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Presentation.Controllers
{

	[Route("api/reports")]
	[Authorize]
	[ApiController]
	public class ReportController : ControllerBase
	{
		private readonly IReportService _reportService;

		public ReportController(IReportService reportService)
		{
			_reportService = reportService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllUserReports()
		{
			var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return BadRequest(ResponseDTO.BadRequest());
			}
			var results = await _reportService.GetAllReportsByUserIdAsync(userId);
			return Ok(ResponseDTO.Accept(result: results));
		}

		[HttpPost]
		public async Task<IActionResult> CreateReport([FromBody] CreateReportDTO createReportDTO)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				if (userId == null)
				{
					throw new UnauthorizedAccessException();
				}
				if (!ModelState.IsValid)
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.InvalidFormatErrorMessage));
				}

				if (createReportDTO.Type != ReportType_COMMENT && createReportDTO.Type != ReportType_RECIPE && createReportDTO.Type != ReportType_USER)
				{
					return BadRequest(ResponseDTO.BadRequest(message: AppString.InvalidReportTypeErrorMessage));
				}

				await _reportService.CreateReportAsync(createReportDTO, userId);
				return Created();
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}

		[HttpPatch]
		public async Task<IActionResult> UpdateReport([FromBody] UpdateReportDTO updatedReportDTO)
		{
			try
			{
				var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
				if (userId == null)
				{
					throw new UnauthorizedAccessException();
				}

				var result = await _reportService.UpdateReportAsync(updatedReportDTO, userId);
				return Ok(ResponseDTO.Accept(result: result));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(ex.Message));
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteReport(string id)
		{
			var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				return BadRequest(ResponseDTO.BadRequest(message: new UnauthorizedAccessException().Message));
			}
			else
			{
				var result = await _reportService.DeleteReportAsync(id, userId);

				if (result == false)
				{
					return BadRequest(ResponseDTO.BadRequest(message: $"Something went wrong when we try to delete the report with id {id}."));
				}
				else
				{
					return Ok(ResponseDTO.Accept());
				}
			}
		}
	}

}
