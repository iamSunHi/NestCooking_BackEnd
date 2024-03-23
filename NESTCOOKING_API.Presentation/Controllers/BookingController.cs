using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.Authentication;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;

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

        [HttpGet("chefs")]
        public async Task<IActionResult> GetAllChefsAsync()
        {
            try
            {
                var result = await _bookingService.GetAllChefsAsync();
                return Ok(ResponseDTO.Accept(result: result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }

        [HttpGet("chefs/{chefId}/dishes")]
        public async Task<IActionResult> GetAllRecipesForBookingByChefIdAsync([FromRoute] string chefId)
        {
            try
            {
                var result = await _recipeService.GetAllRecipesForBookingByChefIdAsync(chefId);
                return Ok(ResponseDTO.Accept(result: result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }
        [HttpGet("chefs/{chefId}/schedules")]
        public async Task<IActionResult> GetChefBookingScheduleDTOs(string chefId)
        {
            try
            {
                var result = await _bookingService.GetChefBookingScheduleDTOs(chefId);
                return Ok(ResponseDTO.Accept(result: result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }

        [HttpGet("my-booking")]
        public async Task<IActionResult> GetAllBookingsByUserIdAsync()
        {
            try
            {
                var user = AuthenticationHelper.GetUserIdFromContext(HttpContext);
                var result = await _bookingService.GetAllBookingsByUserIdAsync(user);
                return Ok(ResponseDTO.Accept(result: result.OrderByDescending(b => b.CreatedAt).ThenByDescending(b => b.Status)));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }

        [HttpGet("chefs/my-booking")]
        [Authorize(Roles = StaticDetails.Role_Chef)]
        public async Task<IActionResult> GetAllBookingsByChefIdAsync()
        {
            try
            {
                var chefId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
                var result = await _bookingService.GetAllBookingsByChefIdAsync(chefId);
                return Ok(ResponseDTO.Accept(result: result.OrderByDescending(b => b.CreatedAt).ThenByDescending(b => b.Status)));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingAsync([FromQuery] string? bookingId)
        {
            try
            {
                if (string.IsNullOrEmpty(bookingId))
                {
                    var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
                    var result = await _bookingService.GetAllBookingsByUserIdAsync(userId);
                    return Ok(ResponseDTO.Accept(result: result));
                }
                else
                {
                    var result = await _bookingService.GetBookingByIdAsync(bookingId);
                    return Ok(ResponseDTO.Accept(result: result));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO createBookingDTO)
        {
            try
            {
                var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
                var result = await _bookingService.CreateBooking(userId, createBookingDTO);
                return Ok(ResponseDTO.Accept(result: result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }

        [HttpPut("update/status")]
        public async Task<IActionResult> UpdateBookingStatusAsync([FromBody] BookingStatusDTO bookingStatusDTO)
        {
            try
            {
                var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);

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
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }

    }
}
