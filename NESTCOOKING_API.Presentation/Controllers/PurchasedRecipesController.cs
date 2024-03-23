using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.PaymentDTOs;
using NESTCOOKING_API.Business.DTOs.TransactionDTOs;
using NESTCOOKING_API.Business.Services;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using NESTCOOKING_API.Utility;
using System.Security.Claims;
using System.Transactions;

namespace NESTCOOKING_API.Presentation.Controllers
{
    [Route("api/purchasedrecipes")]
    [ApiController]
    public class PurchasedRecipesController : ControllerBase
    {
        private readonly IPurchasedRecipesService _purchasedRecipesService;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        public PurchasedRecipesController(IPurchasedRecipesService purchasedRecipesService, ITransactionService transactionService, IUserService userService, IPaymentService paymentService)
        {
            _purchasedRecipesService = purchasedRecipesService;
            _transactionService = transactionService;
            _userService = userService;
            _paymentService = paymentService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePurchasedRecipe(string typeTransaction, string recipeId, TransactionInfor transactionInfor)
        {
            try
            {
                // Validate Order Type
                if (!String.Equals(transactionInfor.OrderType, StaticDetails.PaymentType_PURCHASEDRECIPE, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(ResponseDTO.BadRequest(message: "Transaction type is not valid"));
                }

                // Validate Transaction Type
                if (!String.Equals(typeTransaction, StaticDetails.Payment_Wallet, StringComparison.OrdinalIgnoreCase) &&
                    !String.Equals(typeTransaction, StaticDetails.Payment_VnPay, StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(ResponseDTO.BadRequest(message: "Type is not valid"));
                }

                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var transactionId = await _transactionService.CreateTransaction(transactionInfor, userId, typeTransaction);

                if (String.Equals(typeTransaction, StaticDetails.Payment_Wallet, StringComparison.OrdinalIgnoreCase))
                {
                    var changeBlanceIsSuccess = await _userService.ChangeUserBalanceByTranPurchased(userId, transactionInfor.Amount, recipeId);
                    if (changeBlanceIsSuccess)
                    {
                        await _purchasedRecipesService.CreatePurchasedRecipe(recipeId, transactionId, userId);
                        await _transactionService.TransactionSuccessById(transactionId, true);
                        return Ok(ResponseDTO.Accept(message: "Purchase Recipe Success"));
                    }
                    else
                    {
                        return BadRequest(ResponseDTO.BadRequest(message: "Purchase Recipe Fail"));
                    }
                }
                else
                {
                    await _purchasedRecipesService.CreatePurchasedRecipe(recipeId, transactionId, userId);
                    var paymentUrl = _paymentService.CreatePaymentUrl(transactionInfor, HttpContext, transactionId);
                    return Ok(ResponseDTO.Accept(result: paymentUrl));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPurchasedRecipesByUserId()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _purchasedRecipesService.GetPurchasedRecipesByUserId(userId);
                if (result == null)
                {
                    return BadRequest(ResponseDTO.BadRequest(message: "Not found Purchased Recipes by user"));
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
