using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);


    Task<RefreshToken?> GetByTokenAsync(
        string token);


    Task SaveChangesAsync();
}