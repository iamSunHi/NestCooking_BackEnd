﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
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
        private readonly IUserService _userService;

        public TransactionController(IPaymentService paymentService,ITransactionService transactionService,IUserService userService)
        {
            _paymentService = paymentService;
            _transactionService = transactionService;
            _userService = userService;
        }
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] TransactionInfor transactionInfor)
        {
            try
            {
                if (!String.Equals(transactionInfor.OrderType, PaymentType_DEPOSIT, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(ResponseDTO.BadRequest(message: "Type is not valid"));
                }
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var transactionId = await _transactionService.CreateTransaction(transactionInfor, userId, false, StaticDetails.Payment_VnPay);
                var paymentUrl = _paymentService.CreatePaymentUrl(transactionInfor, HttpContext, transactionId);
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
                    await _userService.ChangeUserBalanceByTranDeposit(paymentResponse.OrderId, paymentResponse.Amount);
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
        public async Task<IActionResult> GetTransactionsByUserId()
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
