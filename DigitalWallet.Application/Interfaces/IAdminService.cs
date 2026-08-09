using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.DTOs.Admin;

namespace DigitalWallet.Application.Interfaces;

public interface IAdminService
{
    Task<PagedResponse<AdminUserResponse>> GetUsersAsync(
    PaginationRequest request);

    Task<List<AdminTransactionResponse>> GetTransactionsAsync();

    Task<List<AdminAuditLogResponse>> GetAuditLogsAsync();

    Task<bool> FreezeUserAsync(int userId);

    Task<bool> UnfreezeUserAsync(int userId);

    Task<bool> ResetUserPasswordAsync(int userId, AdminResetPasswordRequest request);

    Task<PagedResponse<AdminTransactionResponse>>GetTransactionsAsync(PaginationRequest request);

    Task<PagedResponse<AdminAuditLogResponse>>GetAuditLogsAsync( PaginationRequest request);

    Task<AdminDashboardDto> GetDashboardAsync();

    Task<AdminUserDetailsResponse?> GetUserByIdAsync(int id);
}