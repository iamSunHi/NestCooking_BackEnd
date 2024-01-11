using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Instructor
	{
		[Key]
		public int Id { get; set; }
		public Recipe Recipe { get; set; }
		public string Description { get; set; } = null!;
		public string? ImageUrl { get; set; }
	}
}
