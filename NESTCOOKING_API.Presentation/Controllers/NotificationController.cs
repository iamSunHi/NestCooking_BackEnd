using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NESTCOOKING_API.Business.Authentication;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.NotificationDTOs;
using NESTCOOKING_API.Business.Services.IServices;

namespace NESTCOOKING_API.Presentation.Controllers
{
	[Route("api/notification")]
	[ApiController]
	[Authorize]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationService _notificationService;

		public NotificationController(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllNotificationsAsync()
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

		[HttpPatch]
		public async Task<IActionResult> UpdateNotificationByReceiverIdAsync(NotificationUpdateDTO notificationUpdateDTO)
		{
			try
			{
				var userId = AuthenticationHelper.GetUserIdFromContext(HttpContext);
				if (notificationUpdateDTO.Id == null)
				{
					await _notificationService.UpdateAllNotificationStatusAsync(userId);
				}
				else
				{
					await _notificationService.UpdateNotificationStatusByIdAsync(notificationUpdateDTO.Id, userId);
				}

				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
