using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Payment
	{
		[Key]
		public string Id { get; set; } = null!;
		public User User { get; set; }
		public string Type { get; set; } = null!;
		public double Amount { get; set; }
		public string Description { get; set; } = null!;
		public string Currency { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
	}
}
