using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;


    public RefreshTokenRepository(
        AppDbContext context)
    {
        _context = context;
    }



    public async Task AddAsync(
        RefreshToken refreshToken)
    {
        await _context.RefreshTokens
            .AddAsync(refreshToken);
    }



    public async Task<RefreshToken?> GetByTokenAsync(
        string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(
                x => x.Token == token);
    }



    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}