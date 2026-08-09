namespace DigitalWallet.Application.DTOs;

public class TransferRequest
{
    public string ReceiverEmail { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string IdempotencyKey { get; set; } = string.Empty;
}