using Jake.Test.EsigApi.Application.Common.Results;
using Jake.Test.EsigApi.Application.DTOs;
using Jake.Test.EsigApi.Application.Interfaces;
using Jake.Test.EsigApi.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Commands.CreateESignatureRequest;

public class CreateESignatureRequestCommandHandler : IRequestHandler<CreateESignatureRequestCommand, Result<ESignatureRequestDto>>
{
    private readonly IESignatureRepository _repository;
    private readonly ILogger<CreateESignatureRequestCommandHandler> _logger;

    public CreateESignatureRequestCommandHandler(
        IESignatureRepository repository,
        ILogger<CreateESignatureRequestCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<ESignatureRequestDto>> Handle(CreateESignatureRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = ESignatureRequest.Create(
                request.DocumentName,
                request.DocumentContent,
                request.SignerEmail,
                request.SignerName,
                request.Message);

            var result = await _repository.CreateAsync(entity);
            _logger.LogInformation("Created new e-signature request with ID: {Id}", result.Id);
            
            return Result<ESignatureRequestDto>.Success(ESignatureRequestDto.FromEntity(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create e-signature request");
            return Result<ESignatureRequestDto>.Failure(ex.Message);
        }
    }
} 