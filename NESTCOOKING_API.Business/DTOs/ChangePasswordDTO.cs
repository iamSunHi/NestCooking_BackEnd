using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }
        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm new password")]
        public string ConnfirmPassword { get; set; }

    }
}
