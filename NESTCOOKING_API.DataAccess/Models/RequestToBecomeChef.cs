using System.ComponentModel.DataAnnotations;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class RequestToBecomeChef
	{
		[Key]
		public string Id { get; set; } = null!;
		public User User { get; set; }
		public IEnumerable<string> Skills { get; set; }
		public IEnumerable<string> Reasons { get; set; }
		public IEnumerable<string> AchievementImageUrls { get; set; }
		public IEnumerable<string> AchievementDescriptions { get; set; }
		
	}
}
