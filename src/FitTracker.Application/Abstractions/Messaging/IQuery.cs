using MediatR;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}