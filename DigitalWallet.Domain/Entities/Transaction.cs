using DigitalWallet.Domain.Enums;

namespace DigitalWallet.Domain.Entities;

public class Transaction : BaseEntity
{
    public int SenderWalletId { get; set; }

    public int ReceiverWalletId { get; set; }


    public decimal Amount { get; set; }


    public TransactionType Type { get; set; }

    public TransactionStatus Status { get; set; }


    public Wallet SenderWallet { get; set; } = null!;

    public Wallet ReceiverWallet { get; set; } = null!;

    public string IdempotencyKey { get; set; } = string.Empty;
}