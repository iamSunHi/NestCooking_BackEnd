using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
		public async Task<IActionResult> GetAllNotificationsByReceiverIdAsync(string receiverId)
		{
			try
			{
				var notificationList = await _notificationService.GetAllNotificationsByReceiverIdAsync(receiverId);
				return Ok(ResponseDTO.Accept(result: notificationList));
			}
			catch (Exception ex)
			{
				return BadRequest(ResponseDTO.BadRequest(message: ex.Message));
			}
		}
	}
}
