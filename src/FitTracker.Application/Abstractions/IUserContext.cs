using FitTracker.Domain.Enums;

namespace FitTracker.Application.Abstractions
{
    public interface IUserContext
    {
        Guid UserId { get; }
        UserRole Role { get; }
    }
}
