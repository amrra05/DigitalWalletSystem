using DigitalWallet.Domain.Enums;

namespace DigitalWallet.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Customer;

    public bool IsActive { get; set; } = true;

    public bool EmailVerified { get; set; } = false;


    // Navigation property
    public Wallet Wallet { get; set; } = null!;
}