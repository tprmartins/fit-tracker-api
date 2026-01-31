using Microsoft.EntityFrameworkCore;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infra.Context;

namespace FitTracker.Infra.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _dbContext;

        public UserRepository(DatabaseContext dbContext) =>
            _dbContext = dbContext;

        public void Add(User user) =>
            _dbContext.Set<User>().Add(user);

        public async Task<User?> GetByDocumentAsync(Document document, CancellationToken cancellationToken = default) =>
            await _dbContext
                .Set<User>()
                .FirstOrDefaultAsync(user => user.Document == document, cancellationToken);

        public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
            await _dbContext
                .Set<User>()
                .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        public async Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default) =>
            await _dbContext
                .Set<User>()
                .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        public async Task<bool> IsUserDocumentAlreadyExists(Document document, CancellationToken cancellationToken = default) =>
             await _dbContext
                .Set<User>()
                .AnyAsync(user => user.Document == document, cancellationToken);

        public async Task<bool> IsUserEmailAlreadyExists(Email email, CancellationToken cancellationToken = default) =>
             await _dbContext
                .Set<User>()
                .AnyAsync(user => user.Email == email, cancellationToken);

        public async Task<IEnumerable<User>> GetByRoleAsync(FitTracker.Domain.Enums.UserRole role, CancellationToken cancellationToken = default) =>
             await _dbContext
                .Set<User>()
                .Where(user => user.Role == role)
                .ToListAsync(cancellationToken);

        public async Task<int> CountByRoleAsync(FitTracker.Domain.Enums.UserRole role, CancellationToken cancellationToken = default) =>
             await _dbContext
                .Set<User>()
                .CountAsync(user => user.Role == role, cancellationToken);

        public async Task<User?> GetByRegistrationTokenAsync(string token, CancellationToken cancellationToken = default) =>
            await _dbContext
                .Set<User>()
                .FirstOrDefaultAsync(user => user.RegistrationToken == token, cancellationToken);

        public async Task<int> CountActiveAsync(CancellationToken cancellationToken = default) =>
            await _dbContext
                .Set<User>()
                .CountAsync(user => user.IsActive, cancellationToken);

        public async Task<int> CountBlockedAsync(CancellationToken cancellationToken = default) =>
            await _dbContext
                .Set<User>()
                .CountAsync(user => !user.IsActive, cancellationToken);

        public async Task<int> CountAllAsync(CancellationToken cancellationToken = default) =>
            await _dbContext
                .Set<User>()
                .CountAsync(cancellationToken);

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _dbContext
                .Set<User>()
                .OrderBy(u => u.Name)
                .ToListAsync(cancellationToken);
    }
}
