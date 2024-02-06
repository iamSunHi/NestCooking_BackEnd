using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESTCOOKING_API.Business.DTOs.UserDTOs;
using NESTCOOKING_API.DataAccess.Models;

namespace NESTCOOKING_API.Business.DTOs.ChefRequestDTOs
{
    public class RequestToBecomeChefDTO
    {
        public string RequestChefId { get; set; }
        public string UserID { get; set; }
        public string IdentityImage { get; set; }
        public string FullName { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Reasons { get; set; }
        public List<string> AchievementImageUrls { get; set; }
        public List<string> AchievementDescriptions { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
