using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.BookingDTOs;
using NESTCOOKING_API.Business.DTOs.CommentDTOs;
using NESTCOOKING_API.Business.Services;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using System.Security.Claims;

namespace NESTCOOKING_API.Presentation.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpPost]
        [Authorize]
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
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var result = await _bookingService.GetAllBookingRequests();
                return Ok(ResponseDTO.Accept(result: result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(ex.Message));
            }
        }
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBookingById(string bookingId)
        {
            try
            {
                var result = await _bookingService.GetBookingRequest(bookingId);
                return result != null ? Ok(ResponseDTO.Accept(result: result)) : NotFound(AppString.RequestBookingNotFound);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }
        [HttpDelete("{bookingId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(string bookingId)
        {
            try
            {
                var userId = GetUserIdFromContext(HttpContext);
                await _bookingService.DeleteBookingRequest(userId, bookingId);
                return Ok(ResponseDTO.Accept(result: AppString.DeleteBookingSuccessMessage));
            }
            catch (Exception ex)
            {   
                return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
            }
        }
        [HttpPut("approval/{bookingId}")]
        [Authorize]
        public async Task<IActionResult> ApprovalBooking(string bookingId, [FromBody] ChefApprovalProcessDTO approvalProcessDTO)
        {
            try
            {
                var userId = GetUserIdFromContext(HttpContext);
                var approvalBooking = await _bookingService.ApprovalBooking(bookingId, approvalProcessDTO);
                if (approvalBooking != null)
                {
                    return Ok(ResponseDTO.Accept(AppString.ApprovalBookingSuccessMassage, result: approvalBooking));
                }
                else
                {
                    return NotFound(AppString.RequestCommentNotFound);
                }
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
