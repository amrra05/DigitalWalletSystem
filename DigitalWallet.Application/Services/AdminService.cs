using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.DTOs.Admin;
using DigitalWallet.Application.Interfaces;

namespace DigitalWallet.Application.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;

    private readonly ITransactionRepository _transactionRepository;

    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IAuditLogService _auditLogService;

    private readonly IAdminRepository _adminRepository;


    public AdminService(
    IUserRepository userRepository,
    ITransactionRepository transactionRepository,
    IAuditLogRepository auditLogRepository,
    IAdminRepository adminRepository,
    IAuditLogService auditLogService)
    {
        _userRepository = userRepository;

        _transactionRepository = transactionRepository;

        _auditLogRepository = auditLogRepository;

        _auditLogService = auditLogService;
        _adminRepository = adminRepository;
    }


    public async Task<PagedResponse<AdminUserResponse>> GetUsersAsync(
    PaginationRequest request)
    {
        var users =
            await _userRepository.GetPagedAsync(request);



        var totalRecords =
            await _userRepository.CountAsync(request);



        var items =
            users.Select(u => new AdminUserResponse
            {
                Id = u.Id,

                Name = u.Name,

                Email = u.Email,

                Role = u.Role.ToString(),

                Balance = u.Wallet?.Balance ?? 0,

                CreatedAt = u.CreatedAt,

                IsActive = u.IsActive

            })
            .ToList();



        return new PagedResponse<AdminUserResponse>
        {
            Items = items,

            Page = request.Page,

            PageSize = request.PageSize,

            TotalRecords = totalRecords,

            TotalPages =
                (int)Math.Ceiling(
                    totalRecords /
                    (double)request.PageSize)
        };
    }

    public async Task<AdminUserDetailsResponse?> GetUserByIdAsync(int id)
    {

        var user =
            await _userRepository.GetByIdAsync(id);


        if (user == null)
        {
            return null;
        }


        return new AdminUserDetailsResponse
        {

            Id = user.Id,

            Name = user.Name,

            Email = user.Email,

            Role = user.Role.ToString(),

            Balance = user.Wallet?.Balance ?? 0,

            CreatedAt = user.CreatedAt,

            IsActive = user.IsActive

        };

    }

    public async Task<List<AdminTransactionResponse>> GetTransactionsAsync()
    {
        var transactions =
            await _transactionRepository.GetAllAsync();


        return transactions
            .Select(t => new AdminTransactionResponse
            {
                Id = t.Id,

                Amount = t.Amount,

                Type = t.Type.ToString(),

                Status = t.Status.ToString(),

                SenderWalletId = t.SenderWalletId,

                ReceiverWalletId = t.ReceiverWalletId,

                CreatedAt = t.CreatedAt

            })
            .ToList();
    }

    public async Task<List<AdminAuditLogResponse>> GetAuditLogsAsync()
    {
        var logs =
            await _auditLogRepository.GetAllAsync();


        return logs
            .Select(log => new AdminAuditLogResponse
            {
                Id = log.Id,

                UserId = log.UserId,

                Action = log.Action,

                Description = log.Description,

                CreatedAt = log.CreatedAt

            })
            .ToList();
    }

    public async Task<bool> FreezeUserAsync(int userId)
    {
        var user =
            await _userRepository.GetByIdAsync(userId);


        if (user == null)
        {
            return false;
        }


        user.IsActive = false;


        await _userRepository.SaveChangesAsync();


        return true;
    }

    public async Task<bool> UnfreezeUserAsync(int userId)
    {
        var user =
            await _userRepository.GetByIdAsync(userId);


        if (user == null)
        {
            return false;
        }


        user.IsActive = true;


        await _userRepository.SaveChangesAsync();


        return true;
    }

    public async Task<bool> ResetUserPasswordAsync(
    int userId,
    AdminResetPasswordRequest request)
    {
        var user =
            await _userRepository.GetByIdAsync(userId);


        if (user == null)
        {
            return false;
        }


        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(
                request.NewPassword);


        await _userRepository.SaveChangesAsync();


        await _auditLogService.LogAsync(
            userId,
            "PASSWORD_RESET_BY_ADMIN",
            "Admin reset user password."
        );


        return true;
    }

    public async Task<PagedResponse<AdminTransactionResponse>> GetTransactionsAsync(
    PaginationRequest request)
    {
        var transactions =
            await _transactionRepository.GetPagedAsync(request);


        var totalRecords =
            await _transactionRepository.CountAsync(request);


        var items =
            transactions.Select(t => new AdminTransactionResponse
            {
                Id = t.Id,

                Amount = t.Amount,

                Type = t.Type.ToString(),

                Status = t.Status.ToString(),

                SenderWalletId = t.SenderWalletId,

                ReceiverWalletId = t.ReceiverWalletId,

                SenderEmail =
                    t.SenderWallet.User.Email,

                ReceiverEmail =
                    t.ReceiverWallet.User.Email,

                CreatedAt = t.CreatedAt

            })
            .ToList();


        return new PagedResponse<AdminTransactionResponse>
        {
            Items = items,

            Page = request.Page,

            PageSize = request.PageSize,

            TotalRecords = totalRecords,

            TotalPages =
                (int)Math.Ceiling(
                    totalRecords /
                    (double)request.PageSize)
        };
    }

    public async Task<PagedResponse<AdminAuditLogResponse>>GetAuditLogsAsync(PaginationRequest request)
    {
        var logs =
            await _auditLogRepository
                .GetPagedAsync(request);



        var total =
            await _auditLogRepository
                .CountAsync(request);



        var items =
            logs.Select(log =>
            new AdminAuditLogResponse
            {
                Id = log.Id,

                UserId = log.UserId,

                Action = log.Action,

                Description = log.Description,

                CreatedAt = log.CreatedAt

            })
            .ToList();



        return new PagedResponse<AdminAuditLogResponse>
        {
            Items = items,

            Page = request.Page,

            PageSize = request.PageSize,

            TotalRecords = total,

            TotalPages =
                (int)Math.Ceiling(
                    total /
                    (double)request.PageSize)
        };
    }

    public async Task<AdminDashboardDto> GetDashboardAsync()
    {
        return new AdminDashboardDto
        {
            TotalUsers =
                await _adminRepository.GetTotalUsersAsync(),

            ActiveUsers =
                await _adminRepository.GetActiveUsersAsync(),

            FrozenUsers =
                await _adminRepository.GetFrozenUsersAsync(),

            TotalTransactions =
                await _adminRepository.GetTotalTransactionsAsync(),

            TotalWalletBalance =
                await _adminRepository.GetTotalWalletBalanceAsync(),

            RecentTransactions =
                await _adminRepository.GetRecentTransactionsAsync(5)
        };
    }
}