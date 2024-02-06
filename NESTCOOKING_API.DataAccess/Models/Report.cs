﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Report
	{
		[Key]
		public string Id { get; set; } = null!;
		public User User { get; set; }
		public string Target { get; set; }
        [ForeignKey(nameof(Target))]
        [ValidateNever]
        public User? ReportedUser { get; set; }
        public string Title { get; set; } = null!;
		public string Type { get; set; } = null!;
		public string Content { get; set; } = null!;
		public string? ImageUrl { get; set; }
		public string? Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public Response? Response { get; set; }
	}
}
