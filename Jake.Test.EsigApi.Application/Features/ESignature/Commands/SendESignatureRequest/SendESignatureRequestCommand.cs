using Jake.Test.EsigApi.Application.Common.Models;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Commands.SendESignatureRequest;

public record SendESignatureRequestCommand(Guid Id) : ICommand; 