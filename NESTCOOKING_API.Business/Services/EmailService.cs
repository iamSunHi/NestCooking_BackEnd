using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NESTCOOKING_API.Business.DTOs;
using NESTCOOKING_API.Business.DTOs.EmailDTO;
using NESTCOOKING_API.Business.DTOs.ResetPassword;
using NESTCOOKING_API.Business.Services.IServices;
using NESTCOOKING_API.DataAccess.Models;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using NESTCOOKING_API.Utility;
using Azure.Core;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Security;
using NESTCOOKING_API.DataAccess.Repositories.IRepositories;

namespace NESTCOOKING_API.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailRequest _emailRequest;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;

        public EmailService(EmailRequest emailRequest, IAuthService authService, IConfiguration configuration,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager , IUserRepository userRepository)

        {
            _emailRequest = emailRequest;
            _authService = authService;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;

        }

        public void SendEmail(EmailResponse message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }


        private MimeMessage CreateEmailMessage(EmailResponse message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(AppString.NameEmailOwnerDisplay, _emailRequest.From));
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
