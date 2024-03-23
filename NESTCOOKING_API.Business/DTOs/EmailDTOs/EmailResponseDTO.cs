using MimeKit;
using NESTCOOKING_API.Utility;
namespace NESTCOOKING_API.Business.DTOs.EmailDTOs
{
    public class EmailResponseDTO
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public EmailResponseDTO(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(AppString.NameEmailOwnerDisplay, x)));
            Subject = subject;
            Content = content;

        }


    }
}
