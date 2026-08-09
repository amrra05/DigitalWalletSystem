namespace DigitalWallet.Application.DTOs.Admin;

public class AdminTransactionResponse
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public int SenderWalletId { get; set; }

    public int ReceiverWalletId { get; set; }

    public string SenderEmail { get; set; } = string.Empty;

    public string ReceiverEmail { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}