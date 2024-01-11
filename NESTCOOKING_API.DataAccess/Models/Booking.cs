using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Booking
	{
		[Key]
		public string Id { get; set; } = null!;
		public User User { get; set; }
		public User Chef { get; set; }
		public string? Status { get; set; }
		public double Price { get; set; }
		public Payment Payment { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
