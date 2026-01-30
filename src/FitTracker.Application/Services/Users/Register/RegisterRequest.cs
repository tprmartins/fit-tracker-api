using FitTracker.Domain.Enums;

namespace FitTracker.Application.Services.Users.Register
{
    public record class RegisterRequest(string Document, string Password, string Name, string Email, string Phone, UserRole Role)
    {
    }
}