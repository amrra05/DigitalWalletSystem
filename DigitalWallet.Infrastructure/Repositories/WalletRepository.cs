using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Enums;
using DigitalWallet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastructure.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly AppDbContext _context;


    public WalletRepository(AppDbContext context)
    {
        _context = context;
    }



    public async Task<Wallet?> GetByUserIdAsync(int userId)
    {
        return await _context.Wallets
            .FirstOrDefaultAsync(
                w => w.UserId == userId
            );
    }

    public async Task<Wallet?> GetByEmailAsync(string email)
    {
        return await _context.Wallets
            .Include(w => w.User)
            .FirstOrDefaultAsync(w => w.User.Email == email);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        return Task.CompletedTask;
    }

    public async Task<Wallet?> GetSystemWalletAsync()
    {
        return await _context.Wallets
            .Include(w => w.User)
            .FirstOrDefaultAsync(
                w => w.User.Role == UserRole.System);
    }
}