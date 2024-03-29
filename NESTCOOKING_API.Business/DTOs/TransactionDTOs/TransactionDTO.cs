﻿using NESTCOOKING_API.Business.DTOs.UserDTOs;

namespace NESTCOOKING_API.Business.DTOs.TransactionDTOs
{
	public class TransactionDTO
	{
		public string Id { get; set; } = null!;
		public UserShortInfoDTO User { get; set; }
		public string Type { get; set; } = null!;
		public double Amount { get; set; }
		public string Description { get; set; } = null!;
		public string Currency { get; set; } = null!;
		public string Payment { get; set; } = null!;
        public bool IsSuccess { get; set; }
        public DateTime CreatedAt { get; set; }
	}
}
