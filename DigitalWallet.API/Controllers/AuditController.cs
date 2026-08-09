using DigitalWallet.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigitalWallet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;


    public AuditController(
        IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }



    [HttpGet("my-history")]
    public async Task<IActionResult> GetMyHistory()
    {
        var userId =
            int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );


        var logs =
            await _auditLogService
                .GetUserLogsAsync(userId);


        return Ok(logs);
    }
}