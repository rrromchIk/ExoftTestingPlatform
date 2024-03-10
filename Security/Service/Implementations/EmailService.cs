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
    private readonly MailSettings _mailSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmailService(ILogger<EmailService> logger, IOptions<MailSettings> mailSettings, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _mailSettings = mailSettings.Value;
    }

    public async Task<bool> SendEmail(string receiverEmail, string userId, string token)
    {
        var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
        var host = _httpContextAccessor.HttpContext.Request.Host.Value;
        var verificationUrl = $"{scheme}://{host}/api/auth/email/verification"
                              + $"?userId={userId}&token={token}";

        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailSettings.SenderMail));
            email.To.Add(MailboxAddress.Parse(receiverEmail));
            email.Subject = MailContentConstants.Subject;
            email.Body = email.Body = new TextPart(TextFormat.Html)
            {
                Text = MailContentConstants.GetBody(verificationUrl)
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.SenderMail, _mailSettings.AuthPassword);

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