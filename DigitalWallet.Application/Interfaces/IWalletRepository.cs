using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Application.Interfaces;

public interface IWalletRepository
{
    Task<Wallet?> GetByUserIdAsync(int userId);

    Task<Wallet?> GetByEmailAsync(string email);

    Task UpdateAsync(Wallet wallet);

    Task SaveChangesAsync();

    Task<Wallet?> GetSystemWalletAsync();
}