using Jake.Test.EsigApi.Application.Common.Models;
using Jake.Test.EsigApi.Application.DTOs;

namespace Jake.Test.EsigApi.Application.Features.ESignature.Queries.GetESignatureRequestById;

public record GetESignatureRequestByIdQuery(Guid Id) : IQuery<ESignatureRequestDto>; 