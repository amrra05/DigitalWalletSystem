namespace DigitalWallet.Application.DTOs;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;


    public string RefreshToken { get; set; } = string.Empty;

    public bool Success { get; set; }

    public string Message { get; set; }= string.Empty;
}