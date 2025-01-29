using FluentValidation;
using Jake.Test.EsigApi.Application.DTOs;

namespace Jake.Test.EsigApi.Application.Validators;

public class CreateESignatureRequestDtoValidator : AbstractValidator<CreateESignatureRequestDto>
{
    public CreateESignatureRequestDtoValidator()
    {
        RuleFor(x => x.DocumentName)
            .NotEmpty()
            .MaximumLength(255)
            .WithMessage("Document name is required and must not exceed 255 characters.");

        RuleFor(x => x.DocumentContent)
            .NotEmpty()
            .Must(BeValidBase64)
            .WithMessage("Document content must be a valid base64 string.");

        RuleFor(x => x.SignerEmail)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid signer email address is required.");

        RuleFor(x => x.SignerName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Signer name is required and must not exceed 100 characters.");

        RuleFor(x => x.Message)
            .MaximumLength(1000)
            .When(x => x.Message != null)
            .WithMessage("Message must not exceed 1000 characters.");
    }

    private bool BeValidBase64(string base64String)
    {
        if (string.IsNullOrWhiteSpace(base64String)) return false;
        try
        {
            Convert.FromBase64String(base64String);
            return true;
        }
        catch
        {
            return false;
        }
    }
} 