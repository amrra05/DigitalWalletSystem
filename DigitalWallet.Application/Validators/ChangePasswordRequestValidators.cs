using DigitalWallet.Application.DTOs;
using FluentValidation;

namespace DigitalWallet.Application.Validators;

public class ChangePasswordRequestValidator
    : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {


        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage(
                "Password must be at least 8 characters.");
    }
}