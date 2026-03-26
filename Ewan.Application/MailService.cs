using Ewan.Core.Models.Dtos.Mail;
using Ewan.Core.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Ewan.Application
{
    

    public class MailService : IMailService
    {
        private readonly MailSettings _options;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<MailSettings> options, ILogger<MailService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));
            message.Sender = MailboxAddress.Parse(_options.Email);
            message.To.Add(MailboxAddress.Parse(email.To));
            message.Subject = email.Subject;

            var builder = new BodyBuilder();

            if (email.IsHtml)
                builder.HtmlBody = email.Body;
            else
                builder.TextBody = email.Body;

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            try
            {
                await smtp.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls, cancellationToken);
                await smtp.AuthenticateAsync(_options.Email, _options.Password, cancellationToken);
                await smtp.SendAsync(message, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipient}", email.To);
                throw;
            }
        }
    }
}
