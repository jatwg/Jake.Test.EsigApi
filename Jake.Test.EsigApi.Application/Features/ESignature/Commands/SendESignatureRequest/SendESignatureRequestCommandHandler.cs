using Jake.Test.EsigApi.Application.Common.Results;
using Jake.Test.EsigApi.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Commands.SendESignatureRequest;

public class SendESignatureRequestCommandHandler : IRequestHandler<SendESignatureRequestCommand, Result>
{
    private readonly IESignatureRepository _repository;
    private readonly ILogger<SendESignatureRequestCommandHandler> _logger;

    public SendESignatureRequestCommandHandler(
        IESignatureRepository repository,
        ILogger<SendESignatureRequestCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result> Handle(SendESignatureRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                return Result.NotFound($"E-signature request with ID {request.Id} not found");
            }

            entity.Send();
            var success = await _repository.UpdateAsync(entity);
            
            if (!success)
            {
                return Result.Failure("Failed to update e-signature request");
            }

            _logger.LogInformation("Successfully sent e-signature request with ID: {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send e-signature request with ID: {Id}", request.Id);
            return Result.Failure(ex.Message);
        }
    }
} 