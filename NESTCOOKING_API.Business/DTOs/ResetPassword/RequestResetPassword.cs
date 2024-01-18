using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.ResetPassword
{
    public class RequestResetPassword
    {
        public string UserName { get; set; }
        public string Email { get; set; }

    }
}
