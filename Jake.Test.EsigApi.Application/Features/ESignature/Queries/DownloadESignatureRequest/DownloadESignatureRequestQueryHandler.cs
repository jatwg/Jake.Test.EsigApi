using Jake.Test.EsigApi.Application.Common.Results;
using Jake.Test.EsigApi.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Queries.DownloadESignatureRequest;

public class DownloadESignatureRequestQueryHandler : IRequestHandler<DownloadESignatureRequestQuery, Result<byte[]>>
{
    private readonly IESignatureRepository _repository;
    private readonly ILogger<DownloadESignatureRequestQueryHandler> _logger;

    public DownloadESignatureRequestQueryHandler(
        IESignatureRepository repository,
        ILogger<DownloadESignatureRequestQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<byte[]>> Handle(DownloadESignatureRequestQuery request, CancellationToken cancellationToken)
    {
        var document = await _repository.GetDocumentAsync(request.Id);
        if (document == null)
        {
            return Result<byte[]>.NotFound($"Document not found for e-signature request with ID {request.Id}");
        }

        return Result<byte[]>.Success(document);
    }
} 