using MediatR;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
