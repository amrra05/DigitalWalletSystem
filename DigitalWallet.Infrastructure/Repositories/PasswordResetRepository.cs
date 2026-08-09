using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastructure.Repositories;

public class PasswordResetRepository
    : IPasswordResetRepository
{
    private readonly AppDbContext _context;


    public PasswordResetRepository(
        AppDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(
        PasswordResetToken token)
    {
        await _context.PasswordResetTokens
            .AddAsync(token);
    }


    public async Task<PasswordResetToken?> GetByTokenAsync(
        string token)
    {
        return await _context.PasswordResetTokens
            .FirstOrDefaultAsync(
                x => x.Token == token);
    }


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}