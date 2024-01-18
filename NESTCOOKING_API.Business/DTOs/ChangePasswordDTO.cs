using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs
{
    public class ChangePasswordDTO
    {
        [JsonPropertyName("current_password")]
        public string CurrentPassword { get; set; }
		[JsonPropertyName("new_password")]
		public string NewPassword { get; set; }
		[JsonPropertyName("confirm_password")]
		public string ConfirmPassword { get; set; }

    }
}
