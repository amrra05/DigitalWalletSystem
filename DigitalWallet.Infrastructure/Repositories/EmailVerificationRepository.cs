using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastructure.Repositories;

public class EmailVerificationRepository
    : IEmailVerificationRepository
{
    private readonly AppDbContext _context;

    public EmailVerificationRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        EmailVerificationToken token)
    {
        await _context.EmailVerificationTokens
            .AddAsync(token);
    }

    public async Task<EmailVerificationToken?> GetByTokenAsync(
        string token)
    {
        return await _context.EmailVerificationTokens
            .FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}