using NESTCOOKING_API.Utility;

namespace NESTCOOKING_API.Business.DTOs
{
    public class LoginWithThirdPartyRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
