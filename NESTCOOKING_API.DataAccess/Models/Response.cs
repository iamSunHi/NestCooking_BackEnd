using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Response
	{
		[Key]
		public string Id { get; set; } = null!;
		public User User { get; set; }
		public string Title { get; set; } = null!;
		public string Content { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
	}
}
