namespace FitTracker.Application.Services.Users.Login
{
    public record class LoginRequest(string? Document, string? Email, string Password)
    {
    }
}
