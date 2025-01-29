using Jake.Test.EsigApi.Application.Common.Results;
using Jake.Test.EsigApi.Application.Interfaces;
using Jake.Test.EsigApi.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Queries.GetESignatureRequestStatus;

public class GetESignatureRequestStatusQueryHandler : IRequestHandler<GetESignatureRequestStatusQuery, Result<ESignatureStatus>>
{
    private readonly IESignatureRepository _repository;
    private readonly ILogger<GetESignatureRequestStatusQueryHandler> _logger;

    public GetESignatureRequestStatusQueryHandler(
        IESignatureRepository repository,
        ILogger<GetESignatureRequestStatusQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<ESignatureStatus>> Handle(GetESignatureRequestStatusQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            return Result<ESignatureStatus>.NotFound($"E-signature request with ID {request.Id} not found");
        }

        return Result<ESignatureStatus>.Success(entity.Status);
    }
} 