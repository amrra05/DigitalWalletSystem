namespace DigitalWallet.Application.DTOs.Admin;


public class AdminUserDetailsResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public string Role { get; set; } = "";

    public decimal Balance { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }
}