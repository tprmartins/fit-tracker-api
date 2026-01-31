using MediatR;

namespace FitTracker.Application.Services.Users.UnblockUser;

public record UnblockUserCommand(Guid UserId) : IRequest<Unit>;
