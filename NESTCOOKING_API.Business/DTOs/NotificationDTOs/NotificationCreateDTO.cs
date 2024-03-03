﻿using System.ComponentModel;

namespace NESTCOOKING_API.Business.DTOs.NotificationDTOs
{
	public class NotificationCreateDTO
	{
		[DefaultValue(null)]
		public string? SenderId { get; set; }
		[DefaultValue(null)]
		public string? ReceiverId { get; set; }
		public string NotificationType { get; set; } = null!;
		public string Content { get; set; } = null!;
	}
}
