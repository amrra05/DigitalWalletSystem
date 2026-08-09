using System.Security.Claims;
using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalWallet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;


    public WalletController(
        IWalletService walletService)
    {
        _walletService = walletService;
    }



    [HttpGet]
    public async Task<IActionResult> GetWallet()
    {
        var userId = GetUserId();


        var wallet =
            await _walletService.GetWalletAsync(userId);


        if (wallet == null)
        {
            return NotFound(new
            {
                message = "Wallet not found"
            });
        }


        return Ok(wallet);
    }




    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(
        DepositRequest request)
    {
        var userId = GetUserId();


        var result =
            await _walletService.DepositAsync(
                userId,
                request.Amount
            );


        if (!result)
        {
            return BadRequest(new
            {
                message = "Deposit failed"
            });
        }


        return Ok(new
        {
            message = "Deposit successful"
        });
    }





    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(
        WithdrawRequest request)
    {
        var userId = GetUserId();


        var result =
            await _walletService.WithdrawAsync(
                userId,
                request.Amount
            );


        if (!result)
        {
            return BadRequest(new
            {
                message = "Withdrawal failed"
            });
        }


        return Ok(new
        {
            message = "Withdrawal successful"
        });
    }





    private int GetUserId()
    {
        var userId =
            User.FindFirst(
                ClaimTypes.NameIdentifier
            )?.Value;


        return int.Parse(userId!);
    }

    [HttpPost("transfer")]
    [Authorize]
    public async Task<IActionResult> Transfer(
    [FromBody] TransferRequest request)
    {
        var userIdClaim =
    User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }


        var userId =
            int.Parse(userIdClaim.Value);



        var result =
            await _walletService.TransferAsync(
                userId,
                request);



        if (!result.Success)
        {
            return BadRequest(result);
        }


        return Ok(result);
    }

    [HttpGet("transactions")]
    [Authorize]
    public async Task<IActionResult> GetTransactions(
    [FromQuery] PaginationRequest request)
    {
        var userId = GetUserId();

        var transactions =
            await _walletService.GetTransactionsAsync(
                userId,
                request);

        return Ok(transactions);
    }

    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Testing middleware");
    }

}