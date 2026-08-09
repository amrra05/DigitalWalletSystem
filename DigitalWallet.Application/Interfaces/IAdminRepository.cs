using DigitalWallet.Application.DTOs.Admin;

public interface IAdminRepository
{

    Task<int> GetTotalUsersAsync();

    Task<int> GetActiveUsersAsync();

    Task<int> GetFrozenUsersAsync();

    Task<int> GetTotalTransactionsAsync();

    Task<decimal> GetTotalWalletBalanceAsync();

    Task<List<RecentTransactionDto>> GetRecentTransactionsAsync(int count);
}