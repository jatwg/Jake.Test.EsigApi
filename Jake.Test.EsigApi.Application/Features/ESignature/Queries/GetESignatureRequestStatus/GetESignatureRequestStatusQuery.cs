using Jake.Test.EsigApi.Application.Common.Models;
using Jake.Test.EsigApi.Domain.Enums;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Queries.GetESignatureRequestStatus;

public record GetESignatureRequestStatusQuery(Guid Id) : IQuery<ESignatureStatus>; 