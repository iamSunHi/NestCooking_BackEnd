using NESTCOOKING_API.Business.DTOs.EmailDTO;
using NESTCOOKING_API.Business.DTOs.ResetPassword;
using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services.IServices
{
    public interface IEmailService
    {
        void SendEmail(EmailResponse message);
    }
}
