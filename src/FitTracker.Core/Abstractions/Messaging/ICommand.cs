using Niloticus.Core.Shared;
using MediatR;

namespace Niloticus.Core.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
