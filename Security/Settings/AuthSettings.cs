﻿namespace Security.Settings;

public class AuthSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int Lifetime { get; set; }
}