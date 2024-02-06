using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.Utility;
using System.Security.Claims;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Presentation.Controllers
{

    [Route("api/reports/")]
    [ApiController] 
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportDTO reportDto)
        {
            var userId = HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest(ResponseDTO.BadRequest());
            }
            if (reportDto == null)
            {
                return BadRequest(ResponseDTO.BadRequest());
            }

            var result = await _reportService.CreateReportAsync(reportDto,userId);

            if (result ==null)
            {
                return BadRequest(ResponseDTO.BadRequest());
            }

            return Ok(ResponseDTO.Accept(result:result));
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(string id)
        {
            var result = await _reportService.DeleteReportAsync(id);

            if (result == false)
            {
                return BadRequest(ResponseDTO.BadRequest());
            }
            else
            {
                return Ok(ResponseDTO.Accept());
            }
        }
        [Authorize]
        [HttpPatch("{id}")] 
        public async Task<IActionResult> UpdateReport(string id, [FromBody] UpdateReportDTO updatedReportDto)
        {
            var result = await _reportService.UpdateReportAsync(id, updatedReportDto);
            if (result == null)
            {
                return BadRequest(ResponseDTO.BadRequest());
            }

            return Ok(ResponseDTO.Accept(result:result));
        }
        [Authorize(Role_Admin)]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllReportsByUserId()
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
