using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class User : IdentityUser
	{
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public bool IsMale { get; set; }
		public string? Address { get; set; }
		public string? AvatarUrl { get; set; }
		public double? BookingPrice { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string RoleId { get; set; } = null!;
	}
}
