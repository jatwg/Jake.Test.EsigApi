using Jake.Test.EsigApi.Application.Common.Models;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Commands.ResendESignatureRequest;

public record ResendESignatureRequestCommand(Guid Id) : ICommand; 