namespace FitTracker.Application.Services.Users.CompleteRegistration
{
    public record CompleteRegistrationRequest(
        string Token,
        string Document,
        string Password);
}
