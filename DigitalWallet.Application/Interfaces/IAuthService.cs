using DigitalWallet.Application.DTOs;

namespace DigitalWallet.Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequest request);

    Task<AuthResponse?> LoginAsync(LoginRequest request);

    Task<RefreshTokenResponse?> RefreshAsync(
    RefreshTokenRequest request);

    Task<bool> LogoutAsync(
    LogoutRequest request);

    Task<string?> ForgotPasswordAsync(
    ForgotPasswordRequest request);


    Task<bool> ResetPasswordAsync(
        ResetPasswordRequest request);

    Task<bool> ChangePasswordAsync(
    int userId,
    ChangePasswordRequest request);

    Task<bool> VerifyEmailAsync(string token);

    Task<UserProfileResponse?> GetCurrentUserAsync(int userId);
}