using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Users.Login;

namespace FitTracker.Application.Services.Users.Me
{
    public sealed record GetMeQuery() : IQuery<UserResponse>;
}
