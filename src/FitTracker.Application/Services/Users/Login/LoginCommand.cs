using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Users.Login
{
    public record class LoginCommand(string? Document, string Password) : ICommand<LoginResponse>
    {
    }
}
