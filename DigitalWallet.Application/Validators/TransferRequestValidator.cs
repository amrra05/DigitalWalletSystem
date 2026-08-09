using DigitalWallet.Application.DTOs;
using FluentValidation;

namespace DigitalWallet.Application.Validators;

public class TransferRequestValidator
    : AbstractValidator<TransferRequest>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.ReceiverEmail)
            .NotEmpty()
            .WithMessage("Receiver email is required.");


        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");


        
    }
}