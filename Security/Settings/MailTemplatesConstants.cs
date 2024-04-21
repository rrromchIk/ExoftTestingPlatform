namespace Security.Settings;

public class MailTemplatesConstants
{
    public string VerifyEmailMailSubject { get; set; } = null!;
    public string VerifyEmailMailTemplatePath { get; set; } = null!;
    public string ForgotPasswordMailSubject { get; set; } = null!;
    public string ForgotPasswordMailTemplatePath { get; set; } = null!;
    public string VerifyEmailLink { get; set; } = null!;
    public string ForgotPasswordLink { get; set; } = null!;
}