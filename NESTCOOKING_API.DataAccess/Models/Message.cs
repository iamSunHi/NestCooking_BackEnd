using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Message
	{
		[Key]
		public int Id { get; set; }
		public User Sender { get; set; }
		public User Receiver { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
