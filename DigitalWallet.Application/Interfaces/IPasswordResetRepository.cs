using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Application.Interfaces;

public interface IPasswordResetRepository
{
    Task AddAsync(PasswordResetToken token);

    Task<PasswordResetToken?> GetByTokenAsync(
        string token);

    Task SaveChangesAsync();
}