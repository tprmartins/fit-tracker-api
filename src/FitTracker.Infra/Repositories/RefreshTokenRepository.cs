using FitTracker.Domain.Entities.RefreshTokens;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infra.Repositories
{
    internal sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DatabaseContext _dbContext;

        public RefreshTokenRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
        }

        public async Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsUsed && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<RefreshToken>> GetAllExpiredOrUsedTokensAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.RefreshTokens
                .Where(x => x.IsUsed || x.IsRevoked || x.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync(cancellationToken);
        }

        public void Add(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Add(refreshToken);
        }

        public void Update(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Update(refreshToken);
        }

        public void RemoveRange(IEnumerable<RefreshToken> refreshTokens)
        {
            _dbContext.RefreshTokens.RemoveRange(refreshTokens);
        }
    }
}
