namespace DigitalWallet.Application.Settings;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;

    public int ExpiryMinutes { get; set; }
}