using MimeKit;
using NESTCOOKING_API.Business.DTOs.EmailDTOs;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.Utility;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace NESTCOOKING_API.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailRequestDTO _emailRequest;

        public EmailService(EmailRequestDTO emailRequest)

        {
            _emailRequest = emailRequest;
        }

        public void SendEmail(EmailResponseDTO message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailResponseDTO message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(AppString.NameEmailOwnerDisplay, _emailRequest.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailRequest.SmtpServer, _emailRequest.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailRequest.UserName, _emailRequest.Password);

                client.Send(mailMessage);
            }
            catch
            {
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
