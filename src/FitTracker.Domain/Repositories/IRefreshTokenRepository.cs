using FitTracker.Domain.Entities.RefreshTokens;
using FitTracker.Domain.Entities.Users;

namespace FitTracker.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
        Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<RefreshToken>> GetAllExpiredOrUsedTokensAsync(CancellationToken cancellationToken = default);
        void Add(RefreshToken refreshToken);
        void Update(RefreshToken refreshToken);
        void RemoveRange(IEnumerable<RefreshToken> refreshTokens);
    }
}
