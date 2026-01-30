using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Enums;

namespace FitTracker.Application.Services.Users.Register
{
    public record class RegisterCommand(string Document, string Password, string Name, string Email, string Phone, UserRole Role) : ICommand<RegisterResponse>
    {
    }
}
