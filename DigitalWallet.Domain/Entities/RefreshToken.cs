namespace DigitalWallet.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;


    public int UserId { get; set; }


    public DateTime ExpiryDate { get; set; }


    public bool IsRevoked { get; set; }


    public User User { get; set; } = null!;
}