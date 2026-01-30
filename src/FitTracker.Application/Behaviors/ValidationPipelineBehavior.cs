using FluentValidation;
using MediatR;
using FitTracker.Domain.Shared;
using System.Reflection;

namespace FitTracker.Application.Behaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validator)
        {
            _validators = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            Error[] errors = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(failure => failure is not null)
                .Select(failure => new Error($"ValidationError.{failure.PropertyName}", failure.ErrorMessage))
                .Distinct()
                .ToArray();

            if (errors.Length > 0)
            {
                return CreateValidationResult<TResponse>(errors);
            }

            return await next();
        }

        private static TResult CreateValidationResult<TResult>(Error[] errors)
            where TResult : Result
        {
            if (typeof(TResult) == typeof(Result))
            {
                return ValidationResult.WithErrors(errors) as TResult;
            }

            MethodInfo? methodInfo = typeof(ValidationResult<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
                .GetMethod(nameof(ValidationResult.WithErrors));

            if (methodInfo != null)
            {
                object validationResult = methodInfo.Invoke(null, new object?[] { errors })!;

                return (TResult)validationResult;
            }

            throw new InvalidOperationException("Invalid validtion error!");
        }
    }
}
