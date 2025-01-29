using Jake.Test.EsigApi.Application.Common.Results;
using Jake.Test.EsigApi.Application.DTOs;
using Jake.Test.EsigApi.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Queries.GetAllESignatureRequests;

public class GetAllESignatureRequestsQueryHandler : IRequestHandler<GetAllESignatureRequestsQuery, Result<IEnumerable<ESignatureRequestDto>>>
{
    private readonly IESignatureRepository _repository;
    private readonly ILogger<GetAllESignatureRequestsQueryHandler> _logger;

    public GetAllESignatureRequestsQueryHandler(
        IESignatureRepository repository,
        ILogger<GetAllESignatureRequestsQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<ESignatureRequestDto>>> Handle(GetAllESignatureRequestsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _repository.GetAllAsync();
            var dtos = entities.Select(ESignatureRequestDto.FromEntity);
            return Result<IEnumerable<ESignatureRequestDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve e-signature requests");
            return Result<IEnumerable<ESignatureRequestDto>>.Failure(ex.Message);
        }
    }
} 