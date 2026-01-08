using System.Net;
using System.Net.Mail;
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

        public void SendOtp(string toEmail, string otp)
        {
            var smtpClient = new SmtpClient
            {
                Host = _configuration["EmailSettings:SmtpServer"],
                Port = int.Parse(_configuration["EmailSettings:Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]
                )
            };

            var message = new MailMessage
            {
                From = new MailAddress(
                    _configuration["EmailSettings:From"],   
                    "Fundoo Notes"
                ),
                Subject = "OTP Verification",
                Body = $"Your OTP is {otp}",
                IsBodyHtml = false
            };
            var host = _configuration["EmailSettings:SmtpServer"];
            if (string.IsNullOrEmpty(host))
            {
                throw new Exception("SMTP HOST IS NULL - CHECK appsettings.json");
            }

            message.To.Add(toEmail);

            smtpClient.Send(message);
        }
    }
}
