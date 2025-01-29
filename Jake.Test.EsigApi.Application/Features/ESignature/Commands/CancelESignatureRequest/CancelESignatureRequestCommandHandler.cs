using Jake.Test.EsigApi.Application.Common.Results;
using Jake.Test.EsigApi.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Commands.CancelESignatureRequest;

public class CancelESignatureRequestCommandHandler : IRequestHandler<CancelESignatureRequestCommand, Result>
{
    private readonly IESignatureRepository _repository;
    private readonly ILogger<CancelESignatureRequestCommandHandler> _logger;

    public CancelESignatureRequestCommandHandler(
        IESignatureRepository repository,
        ILogger<CancelESignatureRequestCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result> Handle(CancelESignatureRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                return Result.NotFound($"E-signature request with ID {request.Id} not found");
            }

            entity.Cancel();
            var success = await _repository.UpdateAsync(entity);
            
            if (!success)
            {
                return Result.Failure("Failed to update e-signature request");
            }

            _logger.LogInformation("Successfully cancelled e-signature request with ID: {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cancel e-signature request with ID: {Id}", request.Id);
            return Result.Failure(ex.Message);
        }
    }
} 