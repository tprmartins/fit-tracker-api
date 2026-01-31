using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Users
{
    public sealed record GetAllUsersQuery() : IQuery<IEnumerable<UserAdminResponse>>;
}
