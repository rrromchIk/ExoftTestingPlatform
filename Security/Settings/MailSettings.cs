namespace Security.Settings;

public class MailSettings
{
    public string SenderMail { get; set; } = null!;
    public string AuthPassword { get; set; } = null!;
    public string SmtpHost { get; set; } = null!;
    public int SmtpPort { get; set; }
}