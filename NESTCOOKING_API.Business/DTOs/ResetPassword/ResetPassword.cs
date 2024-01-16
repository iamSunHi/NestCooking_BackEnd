using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.ResetPassword
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; } = null!;


        [Compare("Password", ErrorMessage = "The Password And Comfirmation Password Do Not Matching,")]
        public string ConfirmPassword { get; set; } = null!;

        public string Token { get; set; } = null;
        public string Email { get; set; } = null;
    }
}
