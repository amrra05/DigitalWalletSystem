using DigitalWallet.Application.DTOs;

namespace DigitalWallet.Application.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(
        int userId,
        string action,
        string description);


    Task<List<AuditLogResponse>> GetUserLogsAsync(int userId);
}