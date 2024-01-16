using NESTCOOKING_API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs
{
    public class FacebookRequestDTO
    {
        public StaticDetails.Provider LoginProvider { get; set; }
        public string UserName { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
