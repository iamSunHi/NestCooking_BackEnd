using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.DataAccess.Models
{
	public class RequestToBecomeChef
	{
		[Key]
		public string RequestChefId { get; set; } = null!;
		public string UserID { get; set; }
		public string IdentityImage { get; set; }
		public string FullName { get; set; }
		public List<string> Skills { get; set; }
		public List<string> Reasons { get; set; }
		public List<string> AchievementImageUrls { get; set; }
		public List<string> AchievementDescriptions { get; set; }
		public string Status { get; set; }
		public string? ResponseId { get; set; }
		public DateTime CreatedAt { get; set; }
		[JsonIgnore]
		public User User { get; set; }

	}
}
