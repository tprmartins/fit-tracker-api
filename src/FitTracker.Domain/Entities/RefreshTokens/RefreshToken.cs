using FitTracker.Domain.Entities.Users;

namespace FitTracker.Domain.Entities.RefreshTokens
{
    public sealed class RefreshToken
    {
        public RefreshToken(Guid id, UserId userId, string tokenHash, DateTime expiresAt)
        {
            Id = id;
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
            IsUsed = false;
            IsRevoked = false;
        }

        private RefreshToken() 
        { 
            UserId = default!;
            TokenHash = default!;
        }

        public Guid Id { get; private set; }
        public UserId UserId { get; private set; }
        public string TokenHash { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsUsed { get; private set; }
        public bool IsRevoked { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsUsed && !IsExpired;

        public void Use()
        {
            IsUsed = true;
        }

        public void Revoke()
        {
            IsRevoked = true;
        }
    }
}
