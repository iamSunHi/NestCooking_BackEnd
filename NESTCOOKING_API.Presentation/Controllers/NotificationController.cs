using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.Authentication;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.Services.IServices;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/notification")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationService _notificationService;

		public NotificationController(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetAllNotificationsByReceiverIdAsync()
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				var notificationList = await _notificationService.GetAllNotificationsByReceiverIdAsync(userId);
				return Ok(ResponseDTO.Accept(result: notificationList));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}

		[HttpGet("seenAll")]
		[Authorize]
		public async Task<IActionResult> SeenAllUserNotificationsAsync()
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				await _notificationService.SeenAllUserNotificationsAsync(userId);
				return Ok(ResponseDTO.Accept("Success"));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
