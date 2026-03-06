using System.Net;
using System.Net.Mail;
using seed_store_api.Store.Support.Email.Interfaces;

namespace seed_store_api.Store.Support.Email.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _from;
        private readonly string _appPassword;
        private readonly string _host;
        private readonly int _port;
        private readonly string _senderName;

        public EmailService(IConfiguration configuration)
        {
            _from = configuration["Email:From"]!;
            _appPassword = configuration["Email:AppPassword"]!;
            _host = configuration["Email:Host"]!;
            _port = int.Parse(configuration["Email:Port"]!);
            _senderName = configuration["Email:SenderName"]!;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_from, _senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            using var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_from, _appPassword),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}