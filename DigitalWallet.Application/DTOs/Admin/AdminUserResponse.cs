namespace DigitalWallet.Application.DTOs.Admin;

public class AdminUserResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public decimal Balance { get; set; }

    public bool IsActive { get; set; }
}