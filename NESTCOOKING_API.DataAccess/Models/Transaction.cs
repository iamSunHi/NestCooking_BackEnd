using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Transaction
    {
		[Key]
		public string Id { get; set; } = null!;
		public string UserId { get; set; }
        public User User { get; set; }
        public string Type { get; set; } = null!;
		public double Amount { get; set; }
		public string Description { get; set; } = null!;
		public string Currency { get; set; } = null!;
		public string Payment { get; set; } = null!;
		public bool isSuccess { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
