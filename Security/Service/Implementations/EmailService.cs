using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Security.Service.Abstractions;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Security.Settings;

namespace Security.Service.Implementations;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly MailSendingSettings _mailSendingSettings;

    public EmailService(ILogger<EmailService> logger, IOptions<MailSendingSettings> mailSettings)
    {
        _logger = logger;
        _mailSendingSettings = mailSettings.Value;
    }

    public async Task<bool> SendEmail(string receiverEmail, string subject, string text)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailSendingSettings.SenderMail));
            email.To.Add(MailboxAddress.Parse(receiverEmail));
            email.Subject = subject;
            email.Body = email.Body = new TextPart(TextFormat.Html)
            {
                Text = text
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _mailSendingSettings.SmtpHost,
                _mailSendingSettings.SmtpPort,
                SecureSocketOptions.StartTls
            );
            await smtp.AuthenticateAsync(_mailSendingSettings.SenderMail, _mailSendingSettings.AuthPassword);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("{msg} {stackTrace}", e.Message, e.StackTrace);
            return false;
        }
    }
}