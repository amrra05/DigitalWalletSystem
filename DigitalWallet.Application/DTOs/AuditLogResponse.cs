namespace DigitalWallet.Application.DTOs;

public class AuditLogResponse
{
    public string Action { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}