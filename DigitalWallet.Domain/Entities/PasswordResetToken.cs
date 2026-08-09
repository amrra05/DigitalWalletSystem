namespace DigitalWallet.Domain.Entities;

public class PasswordResetToken : BaseEntity
{
    public int UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiryDate { get; set; }

    public bool IsUsed { get; set; }

    public User User { get; set; } = null!;
}