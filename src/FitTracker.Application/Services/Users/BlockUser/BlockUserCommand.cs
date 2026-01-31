using MediatR;

namespace FitTracker.Application.Services.Users.BlockUser;

public record BlockUserCommand(Guid UserId) : IRequest<Unit>;
