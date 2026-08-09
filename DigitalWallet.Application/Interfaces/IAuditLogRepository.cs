using DigitalWallet.Application.DTOs;
using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Application.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog);

    Task<List<AuditLog>> GetUserLogsAsync(int userId);

    Task<List<AuditLog>> GetAllAsync();

    Task<List<AuditLog>> GetPagedAsync( PaginationRequest request);


    Task<int> CountAsync( PaginationRequest request);
}