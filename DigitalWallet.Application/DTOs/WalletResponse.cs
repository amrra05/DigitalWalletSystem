namespace DigitalWallet.Application.DTOs;

public class WalletResponse
{
    public decimal Balance { get; set; }

    public string Currency { get; set; } = string.Empty;
}