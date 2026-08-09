using BCrypt.Net;
using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using DigitalWallet.Application.Settings;
using System.Security.Cryptography;

namespace DigitalWallet.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly IAuditLogService _auditLogService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IEmailService _emailService;
    private readonly IEmailVerificationRepository _emailVerificationRepository;


    public AuthService(
    IUserRepository userRepository,
    IOptions<JwtSettings> jwtSettings,
    IAuditLogService auditLogService,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordResetRepository passwordResetRepository,
    IEmailVerificationRepository emailVerificationRepository,
    IEmailService emailService)
    {
        _userRepository = userRepository;

        _jwtSettings = jwtSettings.Value;

        _auditLogService = auditLogService;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordResetRepository = passwordResetRepository;
        _emailVerificationRepository = emailVerificationRepository;
        _emailService = emailService;
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];


        using var rng =
            RandomNumberGenerator.Create();


        rng.GetBytes(randomBytes);


        return Convert.ToBase64String(randomBytes);
    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
        new Claim(
    ClaimTypes.NameIdentifier,
    user.Id.ToString()
),

        new Claim(
            JwtRegisteredClaimNames.Email,
            user.Email
        ),

        new Claim(
            ClaimTypes.Role,
            user.Role.ToString()
        )
    };


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
        );


        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );


        var token = new JwtSecurityToken(
            claims: claims,

            expires:
                DateTime.UtcNow.AddMinutes(
                    _jwtSettings.ExpiryMinutes
                ),

            signingCredentials: credentials
        );


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var existingUser =
            await _userRepository.GetByEmailAsync(request.Email);


        if (existingUser != null)
        {
            return false;
        }


        var user = new User
        {
            Name = request.Name,

            Email = request.Email,

            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),

            Role = UserRole.Customer,

            IsActive = false,

            Wallet = new Wallet
            {
                Balance = 0,
                Currency = "JOD"
            }

            
        };


        await _userRepository.AddAsync(user);


        await _userRepository.SaveChangesAsync();

        var verificationToken = Guid.NewGuid().ToString();

        var emailToken = new EmailVerificationToken
        {
            UserId = user.Id,

            Token = verificationToken,

            CreatedAt = DateTime.UtcNow,

            ExpiryDate = DateTime.UtcNow.AddHours(24),

            IsUsed = false
        };

        await _emailVerificationRepository.AddAsync(emailToken);

        await _emailVerificationRepository.SaveChangesAsync();

        var verificationLink =
    $"http://localhost:5173/verify-email?token={verificationToken}";

        await _emailService.SendVerificationEmailAsync(
    user.Email,
    verificationLink);


        await _auditLogService.LogAsync(
            user.Id,
            "REGISTER",
            "User account created."
        );


        return true;
    }




    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user =
            await _userRepository.GetByEmailAsync(request.Email);


        if (user == null)
        {
            return null;
        }

        if (!user.IsActive)
        {
            return null;
        }

        bool passwordValid =
    BCrypt.Net.BCrypt.Verify(
        request.Password,
        user.PasswordHash
    );


        if (!passwordValid)
        {
            return null;
        }


        await _auditLogService.LogAsync(
            user.Id,
            "LOGIN",
            "User logged in successfully."
        );

        if (!user.EmailVerified)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Please verify your email before logging in."
            };
        }

        var accessToken =
    GenerateToken(user);


        var refreshToken =
            new Domain.Entities.RefreshToken
            {
                Token = GenerateRefreshToken(),

                UserId = user.Id,

                ExpiryDate =
                    DateTime.UtcNow.AddDays(7),

                IsRevoked = false,

                CreatedAt = DateTime.UtcNow
            };


        await _refreshTokenRepository
            .AddAsync(refreshToken);


        await _refreshTokenRepository
            .SaveChangesAsync();


        return new AuthResponse
        {
            AccessToken = accessToken,

            RefreshToken = refreshToken.Token
        };
    }

    public async Task<RefreshTokenResponse?> RefreshAsync(
    RefreshTokenRequest request)
    {
        var storedToken =
            await _refreshTokenRepository
                .GetByTokenAsync(
                    request.RefreshToken);


        if (storedToken == null)
        {
            return null;
        }


        if (storedToken.IsRevoked)
        {
            return null;
        }


        if (storedToken.ExpiryDate < DateTime.UtcNow)
        {
            return null;
        }



        var user =
            await _userRepository
                .GetByIdAsync(
                    storedToken.UserId);



        if (user == null)
        {
            return null;
        }



        var newAccessToken =
            GenerateToken(user);



        var newRefreshToken =
            new Domain.Entities.RefreshToken
            {
                Token = GenerateRefreshToken(),

                UserId = user.Id,

                ExpiryDate =
                    DateTime.UtcNow.AddDays(7),

                IsRevoked = false,

                CreatedAt = DateTime.UtcNow
            };



        storedToken.IsRevoked = true;



        await _refreshTokenRepository
            .AddAsync(newRefreshToken);



        await _refreshTokenRepository
            .SaveChangesAsync();



        return new RefreshTokenResponse
        {
            AccessToken = newAccessToken,

            RefreshToken = newRefreshToken.Token
        };
    }

    public async Task<bool> LogoutAsync(
    LogoutRequest request)
    {
        var refreshToken =
            await _refreshTokenRepository
                .GetByTokenAsync(
                    request.RefreshToken);


        if (refreshToken == null)
        {
            return false;
        }


        refreshToken.IsRevoked = true;


        await _refreshTokenRepository
            .SaveChangesAsync();

        return true;
    }

    public async Task<string?> ForgotPasswordAsync(
    ForgotPasswordRequest request)
    {
        var user =
            await _userRepository
                .GetByEmailAsync(request.Email);


        if (user == null)
        {
            return null;
        }


        var token =
            Guid.NewGuid().ToString();



        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,

            Token = token,

            ExpiryDate =
                DateTime.UtcNow.AddMinutes(15),

            IsUsed = false,

            CreatedAt = DateTime.UtcNow
        };



        await _passwordResetRepository
            .AddAsync(resetToken);


        await _passwordResetRepository
            .SaveChangesAsync();



        var resetLink =
            $"http://localhost:5173/reset-password?token={token}";



        var emailRequest = new EmailRequest
        {
            To = user.Email,

            Subject = "Digital Wallet Password Reset",

            Body =
            $@"
        <h2>Password Reset Request</h2>

        <p>Hello {user.Name},</p>

        <p>
        You requested to reset your password.
        </p>

        <p>
        Click the link below:
        </p>

        <a href='{resetLink}'>
            Reset Password
        </a>


        <p>
        This link expires in 15 minutes.
        </p>
        "
        };



        await _emailService
            .SendEmailAsync(emailRequest);



        return token;
    }

    public async Task<bool> ResetPasswordAsync(
    ResetPasswordRequest request)
    {
        var resetToken =
            await _passwordResetRepository
                .GetByTokenAsync(request.Token);


        if (resetToken == null)
        {
            return false;
        }


        if (resetToken.IsUsed)
        {
            return false;
        }


        if (resetToken.ExpiryDate < DateTime.UtcNow)
        {
            return false;
        }


        var user =
            await _userRepository
                .GetByIdAsync(resetToken.UserId);


        if (user == null)
        {
            return false;
        }


        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(
                request.NewPassword);


        resetToken.IsUsed = true;


        await _userRepository.SaveChangesAsync();

        await _passwordResetRepository.SaveChangesAsync();


        return true;
    }

    public async Task<bool> ChangePasswordAsync(
    int userId,
    ChangePasswordRequest request)
    {
        var user =
            await _userRepository.GetByIdAsync(userId);


        if (user == null)
        {
            return false;
        }


        var passwordValid =
            BCrypt.Net.BCrypt.Verify(
                request.CurrentPassword,
                user.PasswordHash);


        if (!passwordValid)
        {
            return false;
        }


        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(
                request.NewPassword);


        await _userRepository.SaveChangesAsync();


        await _auditLogService.LogAsync(
            userId,
            "PASSWORD_CHANGED",
            "User changed password."
        );


        return true;
    }

    public async Task<UserProfileResponse?> GetCurrentUserAsync(
    int userId)
    {
        var user =
            await _userRepository.GetByIdAsync(userId);


        if (user == null)
        {
            return null;
        }


        return new UserProfileResponse
        {
            Id = user.Id,

            Name = user.Name,

            Email = user.Email,

            Role = user.Role.ToString(),

            IsActive = user.IsActive
        };
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        var verificationToken =
            await _emailVerificationRepository
                .GetByTokenAsync(token);


        if (verificationToken == null)
        {
            Console.WriteLine("TOKEN NOT FOUND");
            return false;
        }


        Console.WriteLine($"Token Found: {verificationToken.Token}");
        Console.WriteLine($"Used: {verificationToken.IsUsed}");
        Console.WriteLine($"Expiry: {verificationToken.ExpiryDate}");
        Console.WriteLine($"Now: {DateTime.UtcNow}");



        if (verificationToken.IsUsed)
        {
            Console.WriteLine("TOKEN ALREADY USED");
            return false;
        }


        if (verificationToken.ExpiryDate < DateTime.UtcNow)
        {
            Console.WriteLine("TOKEN EXPIRED");
            return false;
        }


        var user =
            await _userRepository
                .GetByIdAsync(verificationToken.UserId);


        if (user == null)
        {
            Console.WriteLine("USER NOT FOUND");
            return false;
        }


        user.EmailVerified = true;
        user.IsActive = true; // Activate the user upon email verification

        verificationToken.IsUsed = true;


        await _userRepository.SaveChangesAsync();

        await _emailVerificationRepository.SaveChangesAsync();


        Console.WriteLine("EMAIL VERIFIED SUCCESSFULLY");


        return true;
    }
}