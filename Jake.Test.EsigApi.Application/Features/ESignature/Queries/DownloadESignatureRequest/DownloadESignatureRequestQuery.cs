using Jake.Test.EsigApi.Application.Common.Models;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Queries.DownloadESignatureRequest;

public record DownloadESignatureRequestQuery(Guid Id) : IQuery<byte[]>; 