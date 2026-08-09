using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace DigitalWallet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;


    public AuthController(IAuthService authService, IEmailService emailService  )
    {
        _authService = authService;
        _emailService = emailService;
    }



    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequest request)
    {
        var result =
            await _authService.RegisterAsync(request);


        if (!result)
        {
            return BadRequest(new
            {
                message = "Email already exists"
            });
        }


        return Ok(new
        {
            message = "User registered successfully"
        });
    }


    [EnableRateLimiting("AuthLimiter")]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request)
    {
        var token =
            await _authService.LoginAsync(request);


        if (token == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password"
            });
        }


        return Ok(new
        {
            token
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
    RefreshTokenRequest request)
    {
        var result =
            await _authService
                .RefreshAsync(request);


        if (result == null)
        {
            return Unauthorized(
                new
                {
                    message = "Invalid refresh token."
                });
        }


        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
    LogoutRequest request)
    {
        var result =
            await _authService
                .LogoutAsync(request);


        if (!result)
        {
            return BadRequest(
                new
                {
                    message = "Invalid refresh token."
                });
        }


        return Ok(
            new
            {
                message = "Logged out successfully."
            });
    }

    [EnableRateLimiting("AuthLimiter")]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
    ForgotPasswordRequest request)
    {

        await _authService
            .ForgotPasswordAsync(request);


        return Ok(new
        {
            message =
            "If the email exists, a reset link was sent."
        });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
    ResetPasswordRequest request)
    {
        var result =
            await _authService
                .ResetPasswordAsync(request);


        if (!result)
        {
            return BadRequest(new
            {
                message = "Invalid or expired token"
            });
        }


        return Ok(new
        {
            message = "Password reset successfully"
        });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
    ChangePasswordRequest request)
    {
        var userIdClaim =
            User.FindFirst(
                ClaimTypes.NameIdentifier);


        if (userIdClaim == null)
        {
            return Unauthorized();
        }


        var userId =
            int.Parse(userIdClaim.Value);


        var result =
            await _authService.ChangePasswordAsync(
                userId,
                request);


        if (!result)
        {
            return BadRequest(new
            {
                message = "Current password is incorrect."
            });
        }


        return Ok(new
        {
            message = "Password changed successfully."
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);


        if (userIdClaim == null)
        {
            return Unauthorized();
        }


        var userId =
            int.Parse(userIdClaim.Value);


        var user =
            await _authService.GetCurrentUserAsync(userId);


        if (user == null)
        {
            return NotFound(new
            {
                message = "User not found"
            });
        }


        return Ok(user);
    }



    [HttpGet("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        try
        {
            var email = new EmailRequest
            {
                To = "test@gmail.com",

                Subject = "Test Email",

                Body = "<h1>Hello from Digital Wallet</h1>"
            };


            await _emailService.SendEmailAsync(email);

            return Ok("Sent");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [EnableRateLimiting("AuthLimiter")]
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail(
    [FromQuery] string token)
    {
        var result =
            await _authService.VerifyEmailAsync(token);

        if (!result)
        {
            return BadRequest(new
            {
                Success = false,
                Message = "Invalid or expired verification link."
            });
        }

        return Ok(new
        {
            Success = true,
            Message = "Email verified successfully."
        });
    }
}