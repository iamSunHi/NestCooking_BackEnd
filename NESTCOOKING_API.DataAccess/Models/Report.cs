using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Report
	{
		[Key]
		public string Id { get; set; } = null!;
		public User User { get; set; }
		public User Target { get; set; }
		public string Title { get; set; } = null!;
		public string Content { get; set; } = null!;
		public string? ImageUrl { get; set; }
		public string? Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public Response? Response { get; set; }
	}
}
