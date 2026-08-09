namespace DigitalWallet.Application.DTOs;

public class TransferResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public decimal NewBalance { get; set; }
}