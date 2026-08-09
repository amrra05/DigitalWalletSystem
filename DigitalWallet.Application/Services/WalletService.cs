using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Exceptions;
using DigitalWallet.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DigitalWallet.Application.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<WalletService> _logger;


    public WalletService(
    IWalletRepository walletRepository,
    ITransactionRepository transactionRepository,
    IUnitOfWork unitOfWork,
    IAuditLogService auditLogService,
    ILogger<WalletService> logger)
    {
        _walletRepository = walletRepository;

        _transactionRepository = transactionRepository;

        _unitOfWork = unitOfWork;

        _auditLogService = auditLogService;

        _logger = logger;
    }



    public async Task<WalletResponse> GetWalletAsync(int userId)
    {
        var wallet =
            await _walletRepository.GetByUserIdAsync(userId);


        if (wallet == null)
        {
            throw new NotFoundException(
                "Wallet not found.");
        }


        return new WalletResponse
        {
            Balance = wallet.Balance,

            Currency = wallet.Currency
        };
    }





    public async Task<bool> DepositAsync(
    int userId,
    decimal amount)
    {
        if (amount <= 0)
        {
            throw new BadRequestException(
                "Amount must be greater than zero.");
        }


        var wallet =
            await _walletRepository.GetByUserIdAsync(userId);


        if (wallet == null)
        {
            throw new NotFoundException(
                "Wallet not found.");
        }


        var bankWallet =
            await _walletRepository.GetSystemWalletAsync();


        if (bankWallet == null)
        {
            throw new NotFoundException(
                "System wallet not found.");
        }


        if (bankWallet.Balance < amount)
        {
            throw new BadRequestException(
                "System wallet has insufficient balance.");
        }



        try
        {
            await _unitOfWork.BeginTransactionAsync();


            bankWallet.Balance -= amount;

            wallet.Balance += amount;



            var transaction =
                new Domain.Entities.Transaction
                {
                    SenderWalletId = bankWallet.Id,

                    ReceiverWalletId = wallet.Id,

                    Amount = amount,

                    Type = Domain.Enums.TransactionType.Deposit,

                    Status = Domain.Enums.TransactionStatus.Completed,

                    IdempotencyKey =
                        Guid.NewGuid().ToString(),

                    CreatedAt = DateTime.UtcNow
                };


            await _transactionRepository.AddAsync(transaction);



            await _auditLogService.LogAsync(
                userId,
                "DEPOSIT",
                $"Deposited {amount} {wallet.Currency} into wallet."
            );



            await _unitOfWork.CommitAsync();



            _logger.LogInformation(
                "Deposit completed. UserId: {UserId}, TransactionId: {TransactionId}, Amount: {Amount}",
                userId,
                transaction.Id,
                amount
            );


            return true;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();

            throw;
        }
    }





    public async Task<bool> WithdrawAsync(
    int userId,
    decimal amount)
    {
        if (amount <= 0)
        {
            throw new BadRequestException(
                "Amount must be greater than zero.");
        }


        var wallet =
            await _walletRepository.GetByUserIdAsync(userId);


        if (wallet == null)
        {
            throw new NotFoundException(
                "Wallet not found.");
        }


        if (wallet.Balance < amount)
        {
            throw new BadRequestException(
                "Insufficient balance.");
        }


        var bankWallet =
            await _walletRepository.GetSystemWalletAsync();


        if (bankWallet == null)
        {
            throw new NotFoundException(
                "System wallet not found.");
        }



        try
        {
            await _unitOfWork.BeginTransactionAsync();


            wallet.Balance -= amount;

            bankWallet.Balance += amount;



            var transaction =
                new Domain.Entities.Transaction
                {
                    SenderWalletId = wallet.Id,

                    ReceiverWalletId = bankWallet.Id,

                    Amount = amount,

                    Type = Domain.Enums.TransactionType.Withdrawal,

                    Status = Domain.Enums.TransactionStatus.Completed,

                    IdempotencyKey =
                        Guid.NewGuid().ToString(),

                    CreatedAt = DateTime.UtcNow
                };


            await _transactionRepository.AddAsync(transaction);



            await _auditLogService.LogAsync(
                userId,
                "WITHDRAW",
                $"Withdrawn {amount} {wallet.Currency} from wallet."
            );



            await _unitOfWork.CommitAsync();



            _logger.LogInformation(
                "Withdrawal completed. UserId: {UserId}, TransactionId: {TransactionId}, Amount: {Amount}",
                userId,
                transaction.Id,
                amount
            );


            return true;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();

            throw;
        }
    }





    public async Task<TransferResponse> TransferAsync(
        int senderUserId,
        TransferRequest request)
    {
        if (string.IsNullOrEmpty(request.IdempotencyKey))
        {
            request.IdempotencyKey = Guid.NewGuid().ToString();
        }

        var existingTransaction =
            await _transactionRepository
                .GetByIdempotencyKeyAsync(
                    request.IdempotencyKey);



        if (existingTransaction != null)
        {
            var existingSenderWallet =
                await _walletRepository
                    .GetByUserIdAsync(senderUserId);


            return new TransferResponse
            {
                Success = true,

                Message = "Transfer already processed.",

                NewBalance = existingSenderWallet?.Balance ?? 0
            };
        }




        var senderWallet =
            await _walletRepository
                .GetByUserIdAsync(senderUserId);



        if (senderWallet == null)
        {
            throw new NotFoundException(
                "Sender wallet not found.");
        }




        var receiverWallet =
            await _walletRepository
                .GetByEmailAsync(
                    request.ReceiverEmail);



        if (receiverWallet == null)
        {
            throw new NotFoundException(
                "Receiver wallet not found.");
        }




        if (senderWallet.Id == receiverWallet.Id)
        {
            throw new BadRequestException(
                "Cannot transfer money to the same wallet.");
        }




        if (senderWallet.Balance < request.Amount)
        {
            throw new BadRequestException(
                "Insufficient balance.");
        }





        try
        {
            await _unitOfWork.BeginTransactionAsync();



            senderWallet.Balance -= request.Amount;

            receiverWallet.Balance += request.Amount;




            var transaction =
                new Domain.Entities.Transaction
                {
                    SenderWalletId = senderWallet.Id,

                    ReceiverWalletId = receiverWallet.Id,

                    Amount = request.Amount,

                    Type = Domain.Enums.TransactionType.Transfer,

                    Status = Domain.Enums.TransactionStatus.Completed,

                    IdempotencyKey = request.IdempotencyKey,

                    CreatedAt = DateTime.UtcNow
                };



            await _transactionRepository
                .AddAsync(transaction);

            await _auditLogService.LogAsync(
    senderUserId,
    "TRANSFER",
    $"Transferred {request.Amount} {senderWallet.Currency} to wallet {receiverWallet.Id}");



            await _unitOfWork.CommitAsync();


            _logger.LogInformation(
    "Transfer completed. UserId: {UserId}, TransactionId: {TransactionId}, Amount: {Amount}",
    senderUserId,
    transaction.Id,
    request.Amount
);


            return new TransferResponse
            {
                Success = true,

                Message = "Transfer completed successfully.",

                NewBalance = senderWallet.Balance
            };
        }
        catch 
        {
            await _unitOfWork.RollbackAsync();


            throw;
        }
    }





    public async Task<PagedResponse<TransactionResponse>> GetTransactionsAsync(
    int userId,
    PaginationRequest request)
    {
        var wallet =
            await _walletRepository
                .GetByUserIdAsync(userId);


        if (wallet == null)
        {
            throw new NotFoundException(
                "Wallet not found.");
        }



        var transactions =
            await _transactionRepository
                .GetWalletTransactionsAsync(
                    wallet.Id,
                    request);



        var totalRecords =
            await _transactionRepository
                .GetWalletTransactionsCountAsync(
                    wallet.Id,
                    request);



        var items =
            transactions
            .Select(t => new TransactionResponse
            {
                Id = t.Id,

                Amount = t.Amount,

                Type = t.Type.ToString(),

                Status = t.Status.ToString(),

                CreatedAt = t.CreatedAt

            })
            .ToList();



        return new PagedResponse<TransactionResponse>
        {
            Items = items,

            Page = request.Page,

            PageSize = request.PageSize,

            TotalRecords = totalRecords,

            TotalPages =
                (int)Math.Ceiling(
                    totalRecords /
                    (double)request.PageSize)
        };
    }
}