using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.ValueObjects;

namespace FitTracker.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByDocumentAsync(Document document, CancellationToken cancellationToken = default);

        Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

        Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);

        Task<bool> IsUserDocumentAlreadyExists(Document document, CancellationToken cancellationToken = default);

        Task<bool> IsUserEmailAlreadyExists(Email email, CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetByRoleAsync(FitTracker.Domain.Enums.UserRole role, CancellationToken cancellationToken = default);

        Task<User?> GetByRegistrationTokenAsync(string token, CancellationToken cancellationToken = default);

        void Add(User user);
    }
}
