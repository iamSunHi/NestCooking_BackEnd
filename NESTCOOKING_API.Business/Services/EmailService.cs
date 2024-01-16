using MailKit.Net.Smtp;
using MimeKit;
using NESTCOOKING_API.Business.DTOs.EmailDTO;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESTCOOKING_API.Business.Services
{
    public class EmailService : IEmailService
    {

        private readonly EmailRequest _emailRequest;

        public EmailService(EmailRequest emailRequest)
        {
            _emailRequest = emailRequest;

        }
        public void SendEmail(EmailResponse emailResponse)
        {
            var emailMessage = CreateEmailMessage(emailResponse);
            Send(emailMessage);
        }


        private MimeMessage CreateEmailMessage(EmailResponse message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Confirm Email From NestCooking", _emailRequest.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }


        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailRequest.SmtpServer, _emailRequest.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailRequest.Username, _emailRequest.Password);

                client.Send(mailMessage);
            }
            catch
            {

                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

    }
}
