using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Security.Settings;

public class AuthSettings
{
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int AccessTokenExpirationMinutes { get; set; }
    public SecurityKey SymmetricSecurityKey 
        => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
}