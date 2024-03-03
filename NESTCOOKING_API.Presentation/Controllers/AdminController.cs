using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.AdminDTOs;
using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using NESTCOOKING_API.Business.Services;
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
        private readonly ITransactionService _transactionService;
        public AdminController(IResponseService responseService, IReportService reportService,ITransactionService transactionService)
        {
            _responseService = responseService;
            _reportService = reportService;
            _transactionService = transactionService;
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
        [HttpGet("transaction")]
        public async Task<ActionResult<List<TransactionDTO>>> GetAllTransactions()
        {
            try
            {
                var result = await _transactionService.GetAllTransactions();
                if (result == null)
                {
                    return BadRequest(ResponseDTO.BadRequest(message: "Not found transaction by user"));
                }
                return Ok(ResponseDTO.Accept(result: result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }

    }
}
