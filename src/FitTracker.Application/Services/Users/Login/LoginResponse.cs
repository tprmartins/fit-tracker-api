namespace FitTracker.Application.Services.Users.Login
{
    public sealed record LoginResponse(string AccessToken, string RefreshToken, UserResponse? User = null)
    {
    }

    public sealed record UserResponse(string Name, string Id, string Document, string Email, string Phone, int Role, int Status)
    {
    }
}
