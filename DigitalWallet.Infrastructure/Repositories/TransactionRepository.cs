using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }

    public async Task<List<Transaction>> GetUserTransactionsAsync(int walletId)
    {
        return await _context.Transactions
            .Where(t =>
                t.SenderWalletId == walletId ||
                t.ReceiverWalletId == walletId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdempotencyKeyAsync(string key)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(
                t => t.IdempotencyKey == key);
    }

    public async Task<List<Transaction>> GetWalletTransactionsAsync(
    int walletId,
    PaginationRequest request)
    {
        var query =
            _context.Transactions
                .Where(t =>
                    t.SenderWalletId == walletId ||
                    t.ReceiverWalletId == walletId);



        // Filtering by type
        if (!string.IsNullOrWhiteSpace(request.Type))
        {
            query = query.Where(t =>
                t.Type.ToString()
                == request.Type);
        }



        // Filtering by status
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(t =>
                t.Status.ToString()
                == request.Status);
        }



        // Sorting
        query =
            request.SortBy.ToLower()
            switch
            {
                "amount" =>
                    request.SortOrder.ToLower() == "asc"
                        ? query.OrderBy(t => t.Amount)
                        : query.OrderByDescending(t => t.Amount),


                _ =>
                    request.SortOrder.ToLower() == "asc"
                        ? query.OrderBy(t => t.CreatedAt)
                        : query.OrderByDescending(t => t.CreatedAt)
            };



        return await query
            .Skip(
                (request.Page - 1)
                * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
    }

    public async Task<int> GetWalletTransactionsCountAsync(
    int walletId,
    PaginationRequest request)
    {
        var query =
            _context.Transactions
                .Where(t =>
                    t.SenderWalletId == walletId ||
                    t.ReceiverWalletId == walletId);



        if (!string.IsNullOrWhiteSpace(request.Type))
        {
            query = query.Where(t =>
                t.Type.ToString()
                == request.Type);
        }



        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(t =>
                t.Status.ToString()
                == request.Status);
        }



        return await query.CountAsync();
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Transactions
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetAdminTransactionsAsync(
    PaginationRequest request)
    {
        var query =
            _context.Transactions
            .Include(t => t.SenderWallet)
            .Include(t => t.ReceiverWallet)
            .AsQueryable();



        if (!string.IsNullOrWhiteSpace(request.Type))
        {
            query = query.Where(t =>
                t.Type.ToString() == request.Type);
        }



        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(t =>
                t.Status.ToString() == request.Status);
        }



        if (request.SortBy.ToLower() == "amount")
        {
            query = request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(t => t.Amount)
                : query.OrderByDescending(t => t.Amount);
        }
        else
        {
            query = request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(t => t.CreatedAt)
                : query.OrderByDescending(t => t.CreatedAt);
        }



        return await query
            .Skip(
                (request.Page - 1) *
                request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
    }

    public async Task<int> GetAdminTransactionsCountAsync(
    PaginationRequest request)
    {
        var query =
            _context.Transactions
            .AsQueryable();



        if (!string.IsNullOrWhiteSpace(request.Type))
        {
            query = query.Where(t =>
                t.Type.ToString() == request.Type);
        }



        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(t =>
                t.Status.ToString() == request.Status);
        }



        return await query.CountAsync();
    }

    public async Task<List<Transaction>> GetPagedAsync(
    PaginationRequest request)
    {
        var query =
            _context.Transactions
                .Include(t => t.SenderWallet)
                    .ThenInclude(w => w.User)
                .Include(t => t.ReceiverWallet)
                    .ThenInclude(w => w.User)
                .AsQueryable();


        // Search sender or receiver email
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(t =>
                t.SenderWallet.User.Email.Contains(
                    request.Search) ||

                t.ReceiverWallet.User.Email.Contains(
                    request.Search));
        }


        // Filter by transaction type
        if (!string.IsNullOrWhiteSpace(request.Type))
        {
            query = query.Where(t =>
                t.Type.ToString() == request.Type);
        }


        // Filter by transaction status
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(t =>
                t.Status.ToString() == request.Status);
        }


        // Sorting
        if (request.SortBy.ToLower() == "amount")
        {
            query =
                request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(t => t.Amount)
                : query.OrderByDescending(t => t.Amount);
        }
        else
        {
            query =
                request.SortOrder.ToLower() == "asc"
                ? query.OrderBy(t => t.CreatedAt)
                : query.OrderByDescending(t => t.CreatedAt);
        }


        return await query
            .Skip(
                (request.Page - 1) *
                request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync(
    PaginationRequest request)
    {
        var query =
            _context.Transactions
                .Include(t => t.SenderWallet)
                    .ThenInclude(w => w.User)
                .Include(t => t.ReceiverWallet)
                    .ThenInclude(w => w.User)
                .AsQueryable();


        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(t =>
                t.SenderWallet.User.Email.Contains(
                    request.Search) ||

                t.ReceiverWallet.User.Email.Contains(
                    request.Search));
        }


        if (!string.IsNullOrWhiteSpace(request.Type))
        {
            query = query.Where(t =>
                t.Type.ToString() == request.Type);
        }


        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(t =>
                t.Status.ToString() == request.Status);
        }


        return await query.CountAsync();
    }
}