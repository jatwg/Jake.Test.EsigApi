using Jake.Test.EsigApi.Application.Common.Models;
using Jake.Test.EsigApi.Application.DTOs;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Commands.CreateESignatureRequest;

public record CreateESignatureRequestCommand : ICommand<ESignatureRequestDto>
{
    public string DocumentName { get; init; } = default!;
    public string DocumentContent { get; init; } = default!;
    public string SignerEmail { get; init; } = default!;
    public string SignerName { get; init; } = default!;
    public string? Message { get; init; }
} 