using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.ReportDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Presentation.Controllers
{
    [ApiController]
    [Route("api/reports/")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportDTO reportDto)
        {
            if (reportDto == null)
            {
                return BadRequest(ResponseDTO.BadRequest());
            }

            var result = await _reportService.CreateReportAsync(reportDto);

            if (result ==null)
            {
                return BadRequest(ResponseDTO.BadRequest());
            }

            return Ok(ResponseDTO.Accept(result:result));
        }
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
        [HttpGet]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllReportsByUserId(string userId)
        {
            var reports = await _reportService.GetAllReportsByUserIdAsync(userId);
            return Ok(reports);
        }
       
    }

}
