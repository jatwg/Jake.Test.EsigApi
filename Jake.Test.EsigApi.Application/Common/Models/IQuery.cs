using MediatR;
using Jake.Test.EsigApi.Application.Common.Results;

namespace Jake.Test.EsigApi.Application.Common.Models;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
} 