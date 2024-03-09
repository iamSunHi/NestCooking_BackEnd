using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IRecipeService _recipeService;
        public BookingController(IBookingService bookingService, IRecipeService recipeService)
        {
            _bookingService = bookingService;
            _recipeService = recipeService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO createBookingDTO)
        {
            try
            {
                var userId = GetUserIdFromContext(HttpContext);
                var result = await _bookingService.CreateBooking(userId, createBookingDTO);
                return Ok(ResponseDTO.Accept(result: result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }
        [HttpGet("recipes/approvedBooking")]
        public async Task<IActionResult> GetAllRecipesApprovedBooking()
        {
            try
            {
                var result = await _recipeService.GetAllRecipesApprovedForBooking();
                return Ok(ResponseDTO.Accept(result: result));

            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }

        private string GetUserIdFromContext(HttpContext context)
        {
            return context.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
