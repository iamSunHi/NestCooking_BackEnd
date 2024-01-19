using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.DTOs
{
	public class ProviderRequestDTO
    {
        public StaticDetails.Provider LoginProvider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
