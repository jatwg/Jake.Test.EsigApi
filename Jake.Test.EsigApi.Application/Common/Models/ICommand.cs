using MediatR;
using Jake.Test.EsigApi.Application.Common.Results;

namespace Jake.Test.EsigApi.Application.Common.Models;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
} 
