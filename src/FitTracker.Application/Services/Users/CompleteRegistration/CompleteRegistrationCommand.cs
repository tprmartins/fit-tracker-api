using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Users.CompleteRegistration
{
    public record CompleteRegistrationCommand(
        string Token,
        string Document,
        string Password) : ICommand;
}
