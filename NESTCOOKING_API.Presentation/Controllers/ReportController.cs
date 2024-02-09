using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using NESTCOOKING_API.Business.Exceptions;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
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
                var result = await _reportService.CreateReportAsync(createReportDTO, userId);

                if (result == null)
                {
                    return StatusCode(500, AppString.InternalServerErrorMessage);
                }

                return Ok(ResponseDTO.Accept(result: result));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized();
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
            if(userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            else
            {
                var result = await _reportService.DeleteReportAsync(id,userId);

                if (result == false)
                {
                    return BadRequest(ResponseDTO.BadRequest());
                }
                else
                {
                    return Ok(ResponseDTO.Accept());
                }
            }
           
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateReport(string id, [FromBody] UpdateReportDTO updatedReportDto)
        {
            var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            var result = await _reportService.UpdateReportAsync(id, updatedReportDto,userId);
            if (result == null)
            {
                return BadRequest(ResponseDTO.BadRequest());
            }

            return Ok(ResponseDTO.Accept(result: result));
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
    }

}
