using DigitalWallet.Application.DTOs;

namespace DigitalWallet.Application.Interfaces;

public interface IEmailService
{
    Task SendVerificationEmailAsync(
    string email,
    string verificationLink);

    Task SendEmailAsync(
        EmailRequest request
    );

}