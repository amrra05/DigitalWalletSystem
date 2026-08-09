using DigitalWallet.Domain.Entities;

public interface IEmailVerificationRepository
{
    Task AddAsync(EmailVerificationToken token);

    Task<EmailVerificationToken?> GetByTokenAsync(string token);

    Task SaveChangesAsync();
}