using FluentValidation;
using MediatR;
using ValidationException = Jake.Test.EsigApi.Application.Exceptions.ValidationException;

namespace Jake.Test.EsigApi.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToDictionary(
                failure => failure.PropertyName,
                failure => new[] { failure.ErrorMessage }
            );

        if (failures.Any())
            throw new ValidationException(failures);

        return await next();
    }
} 