using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.Services;
using NESTCOOKING_API.Business.Services.IServices;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
    [Route("api/bookings")]
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
        [HttpPut("approvalStatus/{bookingId}")]
        public async Task<IActionResult> UpdateBookingStatus(string bookingId, [FromBody] BookingStatusDTO bookingStatusDTO)
        {
            try
            {
                var userId = GetUserIdFromContext(HttpContext);

                if (bookingId != bookingStatusDTO.Id)
                {
                    return BadRequest(ResponseDTO.BadRequest(message: "Mismatched bookingId in the route and DTO."));
                }
                var result = await _bookingService.UpdateBookingStatus(userId, bookingStatusDTO);
                if (result != null)
                {
                    return Ok(ResponseDTO.Accept(result: result));
                }
                else
                {
                    return BadRequest(ResponseDTO.BadRequest(message: "Failed to update booking status."));
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBooking()
        {
            try
            {
                var result = await _bookingService.GetAllBookings();
                return Ok(ResponseDTO.Accept(result: result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBooking(string bookingId)
        {
            try
            {
                var result = await _bookingService.GetBooking(bookingId);
                return Ok(ResponseDTO.Accept(result: result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
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
