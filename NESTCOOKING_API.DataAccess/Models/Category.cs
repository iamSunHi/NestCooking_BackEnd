using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; } = null!;
	}
}
