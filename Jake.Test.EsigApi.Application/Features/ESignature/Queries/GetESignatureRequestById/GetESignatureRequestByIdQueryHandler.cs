using Jake.Test.EsigApi.Application.Common.Results;
using Jake.Test.EsigApi.Application.DTOs;
using Jake.Test.EsigApi.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Queries.GetESignatureRequestById;

public class GetESignatureRequestByIdQueryHandler : IRequestHandler<GetESignatureRequestByIdQuery, Result<ESignatureRequestDto>>
{
    private readonly IESignatureRepository _repository;
    private readonly ILogger<GetESignatureRequestByIdQueryHandler> _logger;

    public GetESignatureRequestByIdQueryHandler(
        IESignatureRepository repository,
        ILogger<GetESignatureRequestByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<ESignatureRequestDto>> Handle(GetESignatureRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            return Result<ESignatureRequestDto>.NotFound($"E-signature request with ID {request.Id} not found");
        }
        
        return Result<ESignatureRequestDto>.Success(ESignatureRequestDto.FromEntity(entity));
    }
} 