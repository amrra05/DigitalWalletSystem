namespace DigitalWallet.Application.DTOs.Admin;

public class AdminResetPasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}