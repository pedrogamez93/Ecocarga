using Cl.Gob.Energia.Ecocarga.Api.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Api.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly IHostingEnvironment _env;
        private readonly ILogger _logger;

        public EmailSender(
            IOptions<EmailSettings> emailSettings,
            IHostingEnvironment env,
            ILogger<EmailSender> logger)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

                InternetAddressList listEmail = new InternetAddressList();
                string[] emails = email.Split(new Char[] { ';' });

                foreach (string _email in emails)
                {
                    listEmail.Add(new MailboxAddress(_email));
                }

                mimeMessage.To.AddRange(listEmail);
                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    client.CheckCertificateRevocation = false;

                    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, false);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
