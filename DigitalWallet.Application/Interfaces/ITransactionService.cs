using DigitalWallet.Application.DTOs;

namespace DigitalWallet.Application.Interfaces;

public interface ITransactionService
{
    Task<bool> TransferAsync(
        int senderUserId,
        TransferRequest request);

    Task<List<TransactionResponse>> GetHistoryAsync(
        int userId);
}