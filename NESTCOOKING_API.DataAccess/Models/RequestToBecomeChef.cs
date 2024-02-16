using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static NESTCOOKING_API.Utility.StaticDetails;

namespace NESTCOOKING_API.DataAccess.Models
{
    public class RequestToBecomeChef
    {
        [Key]
        public string RequestChefId { get; set; }
        public string UserID { get; set; }
        public string IdentityImageUrl { get; set; }
        public List<string> CertificateImageUrls { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string DOB { get; set; }
        public string Category { get; set; }
        public string Email { get; set; }
        public string Experience { get; set; }
        public string Achievement { get; set; }
        public string Status { get; set; }
        public string? ResponseId { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
