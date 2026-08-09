using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.DTOs.Admin;
using DigitalWallet.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DigitalWallet.API.Controllers;


[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;


    public AdminController(
        IAdminService adminService)
    {
        _adminService = adminService;
    }


    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
    [FromQuery] PaginationRequest request)
    {
        var users =
            await _adminService.GetUsersAsync(request);


        return Ok(users);
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions(
    [FromQuery] PaginationRequest request)
    {
        var result =
            await _adminService
                .GetTransactionsAsync(request);


        return Ok(result);
    }

    [HttpGet("auditlogs")]
    public async Task<IActionResult> GetAuditLogs(
    [FromQuery] PaginationRequest request)
    {
        var logs =
            await _adminService.GetAuditLogsAsync(request);

        return Ok(logs);
    }

    [HttpPut("users/{id}/freeze")]
    public async Task<IActionResult> FreezeUser(int id)
    {
        var result =
            await _adminService.FreezeUserAsync(id);


        if (!result)
        {
            return NotFound();
        }


        return Ok(new
        {
            message = "User frozen successfully"
        });
    }

    [HttpPut("users/{id}/unfreeze")]
    public async Task<IActionResult> UnfreezeUser(int id)
    {
        var result =
            await _adminService.UnfreezeUserAsync(id);


        if (!result)
        {
            return NotFound();
        }


        return Ok(new
        {
            message = "User unfrozen successfully"
        });
    }

    [HttpPut("users/{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(
    int id,
    AdminResetPasswordRequest request)
    {
        var result =
            await _adminService.ResetUserPasswordAsync(
                id,
                request);


        if (!result)
        {
            return NotFound(new
            {
                message = "User not found"
            });
        }


        return Ok(new
        {
            message = "Password reset successfully"
        });
    }

    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user =
            await _adminService.GetUserByIdAsync(id);


        if (user == null)
        {
            return NotFound();
        }


        return Ok(user);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var result =
            await _adminService.GetDashboardAsync();

        return Ok(result);
    }
}