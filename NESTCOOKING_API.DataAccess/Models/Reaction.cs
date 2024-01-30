using System.ComponentModel.DataAnnotations;
namespace NESTCOOKING_API.DataAccess.Models
{
	public class Reaction
	{
		[Key]
		public string Id { get; set; } = null!;
		public string Emoji { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public IEnumerable<Recipe> Recipes { get; set; }
	}
}
