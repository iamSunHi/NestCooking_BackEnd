using System.Text.Json.Serialization;

namespace NESTCOOKING_API.Business.DTOs
{
	public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmPassword { get; set; }

    }
}
