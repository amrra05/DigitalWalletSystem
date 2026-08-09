using DigitalWallet.Application.DTOs;
using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Application.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);

    Task<List<Transaction>> GetUserTransactionsAsync(int walletId);

    Task<Transaction?> GetByIdempotencyKeyAsync(string key);


    Task<List<Transaction>> GetWalletTransactionsAsync(
    int walletId,
    PaginationRequest request);


    Task<int> GetWalletTransactionsCountAsync(
        int walletId,
        PaginationRequest request);

    Task SaveChangesAsync();

    Task<List<Transaction>> GetAllAsync();

    Task<List<Transaction>> GetAdminTransactionsAsync(
    PaginationRequest request);

    Task<List<Transaction>> GetPagedAsync(
        PaginationRequest request);
    Task<int> CountAsync(
        PaginationRequest request);


    Task<int> GetAdminTransactionsCountAsync(
        PaginationRequest request);

}