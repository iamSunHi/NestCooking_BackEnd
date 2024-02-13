using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AdminDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/admin")]
    [ApiController]
    [Authorize(Role_Admin)]
    public class AdminController : ControllerBase

    {
        private readonly IResponseService _responseService;
        private readonly IReportService _reportService;

        public AdminController(IResponseService responseService, IReportService reportService)
        {
            _responseService = responseService;
            _reportService = reportService;
        }
        [HttpPost("reports/{reportId}")]
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
                return BadRequest(ResponseDTO.Create(System.Net.HttpStatusCode.BadRequest, ex.Message));
            }

        }
        [HttpGet("reports")]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                var result = await _reportService.GetAllReportsAsync();
                return Ok(ResponseDTO.Accept(result: result));
            } catch(Exception ex)
            {
                return BadRequest(ResponseDTO.Create(System.Net.HttpStatusCode.BadRequest, ex.Message));
            }
        }
           
    }
}
