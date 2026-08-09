namespace DigitalWallet.Domain.Entities;

public class Wallet : BaseEntity
{
    public int UserId { get; set; }

    public decimal Balance { get; set; }

    public string Currency { get; set; } = "JOD";

    public bool IsFrozen { get; set; }

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    // Navigation property
    public User User { get; set; } = null!;
}