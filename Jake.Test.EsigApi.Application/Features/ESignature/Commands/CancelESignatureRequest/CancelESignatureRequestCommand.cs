using Jake.Test.EsigApi.Application.Common.Models;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Commands.CancelESignatureRequest;

public record CancelESignatureRequestCommand(Guid Id) : ICommand; 