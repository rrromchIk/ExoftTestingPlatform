namespace Security.Settings;

public static class MailContentConstants
{
    public const string Subject = "Email verification";
    
    public static string GetBody(string url)
    {
      return $@"<!DOCTYPE html>
<html>
<head>
  <meta http-equiv=""Content-Type"" content=""text/html"" charset=""UTF-8"" />
  <title>Email Verification</title>
  <link href=""https://fonts.googleapis.com/css2?family=Lato&display=swap"" rel=""stylesheet"">
  <style>
    .button-container {{
      text-align: center;
    }}
    .verify-button {{
      background-color: transparent; /* Transparent button background */
      color: black; /* Text color */
      width: 175px;
      height: 35px;
      font-family: 'Lato', sans-serif;
      font-size: 1em;
      border: 2px solid black; /* Black border */
      border-radius: 20px;
      text-decoration: none;
      display: inline-block;
      line-height: 35px;
      text-align: center;
    }}
    .email-text {{
      color: #333;
      font-size: 1em;
    }}
  </style>
</head>
<body>

  <table cellpadding=""0"" cellspacing=""0"" border=""0"">
    <tr>
      <td>
        <h2>Email Verification</h2>
        <p class=""email-text"">Thank you for signing up! To verify your email address, please click the link below:</p>
      </td>
    </tr>
    <tr>
      <td style=""text-align: center;"">
        <div class=""button-container"">
          <a class=""verify-button"" href=""{url}"">Verify Email</a>
        </div>
      </td>
    </tr>
    <tr>
      <td>
        <p class=""email-text"">If you didn't sign up for this service, please ignore this email.</p>
        <p class=""email-text"" style=""margin-bottom: 5px;"">Best regards,</p>
        <p class=""email-text"" style=""margin: 0;"">Testing platfrom</p>
      </td>
    </tr>
  </table>
</body>
</html>";
    }
}