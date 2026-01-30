using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Users.Login;

namespace FitTracker.Application.Services.Users.GetByRole
{
    public record GetUsersByRoleQuery(int Role) : IQuery<List<UserResponse>>;
}
