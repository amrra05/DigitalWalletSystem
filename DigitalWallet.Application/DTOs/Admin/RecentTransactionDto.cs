namespace DigitalWallet.Application.DTOs.Admin;

public class RecentTransactionDto
{
    public int Id { get; set; }

    public string SenderEmail { get; set; } = string.Empty;

    public string ReceiverEmail { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Type { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}