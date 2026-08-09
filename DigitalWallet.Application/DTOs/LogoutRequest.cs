namespace DigitalWallet.Application.DTOs;

public class LogoutRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}