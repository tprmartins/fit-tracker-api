using Niloticus.Core.Shared;
using MediatR;

namespace Niloticus.Core.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}