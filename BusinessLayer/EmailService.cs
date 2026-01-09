using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOtpAsync(string toEmail, string otp)
        {
            var host = _configuration["EmailSettings:SmtpServer"];
            if (string.IsNullOrEmpty(host))
            {
                throw new Exception("SMTP HOST IS NULL - CHECK appsettings.json");
            }

            using var smtpClient = new SmtpClient
            {
                Host = host,
                Port = int.Parse(_configuration["EmailSettings:Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]
                )
            };

            using var message = new MailMessage
            {
                From = new MailAddress(
                    _configuration["EmailSettings:From"],
                    "Fundoo Notes"
                ),
                Subject = "OTP Verification",
                Body = $"Your OTP is {otp}",
                IsBodyHtml = false
            };

            message.To.Add(toEmail);

            await smtpClient.SendMailAsync(message);
        }
    }
}
