using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.ChefRequestDTOs
{
	public class CreatedRequestToBecomeChefDTO
	{
		public string IdentityImage { get; set; }
		public string FullName { get; set; }
		public List<string> Skills { get; set; }
		public List<string> Reasons { get; set; }
		public List<string> AchievementImageUrls { get; set; }
		public List<string> AchievementDescriptions { get; set; }
	}
}
