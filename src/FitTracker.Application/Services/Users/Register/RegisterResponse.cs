namespace FitTracker.Application.Services.Users.Register
{
    public sealed record RegisterResponse(Guid userId, string AccessToken, string RefreshToken, string Document, string Name, string Email, DateTime CreatedAt)
    {
    }
}
