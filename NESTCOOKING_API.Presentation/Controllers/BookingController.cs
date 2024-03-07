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

		public BookingController(IBookingService bookingService)
		{
			_bookingService = bookingService;
		}

		[HttpPost]
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
		public async Task<IActionResult> DeleteComment(string bookingId)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				await _bookingService.DeleteBookingRequest(userId, bookingId);
				return Ok(ResponseDTO.Accept(result: AppString.DeleteBookingSuccessMessage));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpPut("approve")]
		public async Task<IActionResult> ApprovalBooking([FromBody] ChefApprovalProcessDTO approvalProcessDTO)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				var approvalBooking = await _bookingService.ApprovalBooking(approvalProcessDTO);
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
	}
}
