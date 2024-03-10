using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Security.Settings;

public class AuthSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; }
    public SecurityKey SymmetricSecurityKey { get =>
        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey)); }
}