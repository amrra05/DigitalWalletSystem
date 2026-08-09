using DigitalWallet.Application.DTOs;

namespace DigitalWallet.Application.Interfaces;

public interface IWalletService
{
    Task<WalletResponse> GetWalletAsync(int userId);

    Task<bool> DepositAsync(
        int userId,
        decimal amount
    );

    Task<bool> WithdrawAsync(
        int userId,
        decimal amount
    );

    Task<TransferResponse> TransferAsync(
    int senderUserId,
    TransferRequest request);

    Task<PagedResponse<TransactionResponse>> GetTransactionsAsync(
    int userId,
    PaginationRequest request);
}