using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Application.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;


    public AuditLogService(
        IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }



    public async Task LogAsync(
        int userId,
        string action,
        string description)
    {
        var auditLog = new AuditLog
        {
            UserId = userId,

            Action = action,

            Description = description,

            CreatedAt = DateTime.UtcNow
        };


        await _auditLogRepository
            .AddAsync(auditLog);
    }





    public async Task<List<AuditLogResponse>> GetUserLogsAsync(
    int userId)
    {
        var logs =
            await _auditLogRepository
                .GetUserLogsAsync(userId);


        return logs.Select(log => new AuditLogResponse
        {
            Action = log.Action,

            Description = log.Description,

            CreatedAt = log.CreatedAt

        }).ToList();
    }
}