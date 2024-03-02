using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.PaymentDTOs;
using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Security.Claims;
using static NESTCOOKING_API.Utility.StaticDetails;
namespace NESTCOOKING_API.Presentation.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ITransactionService _transactionService;

        public TransactionController(IPaymentService paymentService,ITransactionService transactionService)
        {
            _paymentService = paymentService;
            _transactionService = transactionService;
        }
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentInfor model)
        {
            try
            {
                if (!String.Equals(model.OrderType, PaymentType_DEPOSIT, StringComparison.OrdinalIgnoreCase) &&!String.Equals(model.OrderType, PaymentType_WITHDRAW, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(ResponseDTO.BadRequest(message: "Type is not valid"));
                }
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var transactionId = await _transactionService.CreateTransaction(model, userId);
                var paymentUrl = _paymentService.CreatePaymentUrl(model, HttpContext, transactionId);
                return Ok(ResponseDTO.Accept(result: paymentUrl));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }

        [HttpGet("callback")]
        public async Task<IActionResult> ProcessPaymentCallback()
        {
            try
            {
                var paymentResponse = _paymentService.ProcessPaymentCallback(Request.Query);
                if (paymentResponse.Success)
                {
                    await _transactionService.TransactionSuccessById(paymentResponse.OrderId);
                }
                return Ok(ResponseDTO.Accept(result: paymentResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }     
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactionsByUserId()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _transactionService.GetTransactionsByUserId(userId);
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
