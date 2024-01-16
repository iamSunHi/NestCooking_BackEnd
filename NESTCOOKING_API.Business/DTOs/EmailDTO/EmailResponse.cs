using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
namespace NESTCOOKING_API.Business.DTOs.EmailDTO
{
    public class EmailResponse
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public EmailResponse(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("Confirm Email From NestCooking", x)));
            Subject = subject;
            Content = content;

        }


    }
}
