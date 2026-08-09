using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.DTOs.Admin;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class  AdminRepository : IAdminRepository
{

    private readonly AppDbContext _context;

    public AdminRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalUsersAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<int> GetActiveUsersAsync()
    {
        return await _context.Users.CountAsync(x => x.IsActive);
    }

    public async Task<int> GetFrozenUsersAsync()
    {
        return await _context.Users.CountAsync(x => !x.IsActive);
    }

    public async Task<int> GetTotalTransactionsAsync()
    {
        return await _context.Transactions.CountAsync();
    }

    public async Task<decimal> GetTotalWalletBalanceAsync()
    {
        return await _context.Wallets.SumAsync(x => x.Balance);
    }

    public async Task<List<RecentTransactionDto>> GetRecentTransactionsAsync(int count)
    {
        return await _context.Transactions

            .Include(t => t.SenderWallet)
                .ThenInclude(w => w.User)

            .Include(t => t.ReceiverWallet)
                .ThenInclude(w => w.User)

            .OrderByDescending(t => t.CreatedAt)

            .Take(count)

            .Select(t => new RecentTransactionDto
            {
                Id = t.Id,

                SenderEmail =
                    t.SenderWallet.User.Email,

                ReceiverEmail =
                    t.ReceiverWallet.User.Email,

                Amount =
                    t.Amount,

                Type =
                    t.Type.ToString(),

                CreatedAt =
                    t.CreatedAt
            })

            .ToListAsync();
    }
}
