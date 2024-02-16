using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.ChefRequestDTOs
{
    public class CreatedRequestToBecomeChefDTO
    {
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
    }
}
