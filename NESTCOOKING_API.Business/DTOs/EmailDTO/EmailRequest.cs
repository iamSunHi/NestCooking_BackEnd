using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.DTOs.EmailDTO
{
    public class EmailRequest
    {
        public string From { get; set; } = null;
        public string SmtpServer { get; set; } = null;

        public int Port { get; set; }

        public string Username { get; set; }=null;
        public string Password { get; set; } = null;


    }
}
