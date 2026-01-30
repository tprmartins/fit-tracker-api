using FitTracker.Application.Abstractions;
using FitTracker.Domain.Entities.RefreshTokens;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FitTracker.IntegrationTests.Security
{
    public class RefreshTokenHashingTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;

        public RefreshTokenHashingTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RefreshToken_ShouldBeHashedBeforeStorage()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtProvider>();
            var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var plainToken = jwtProvider.GenerateRefreshToken();
            var hashedToken = jwtProvider.HashToken(plainToken);

            var userId = new UserId(Guid.NewGuid());
            var refreshToken = new RefreshToken(
                Guid.NewGuid(),
                userId,
                hashedToken,
                DateTime.UtcNow.AddDays(7));

            // Act
            refreshTokenRepository.Add(refreshToken);
            await unitOfWork.SaveChangesAsync();

            // Assert
            var storedToken = await refreshTokenRepository.GetByTokenHashAsync(hashedToken);
            Assert.NotNull(storedToken);
            Assert.Equal(hashedToken, storedToken.TokenHash);
            Assert.NotEqual(plainToken, storedToken.TokenHash); // Ensure it's not stored as plain text
        }

        [Fact]
        public async Task RefreshToken_ShouldNotBeRetrievableWithPlainToken()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtProvider>();
            var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var plainToken = jwtProvider.GenerateRefreshToken();
            var hashedToken = jwtProvider.HashToken(plainToken);

            var userId = new UserId(Guid.NewGuid());
            var refreshToken = new RefreshToken(
                Guid.NewGuid(),
                userId,
                hashedToken,
                DateTime.UtcNow.AddDays(7));

            refreshTokenRepository.Add(refreshToken);
            await unitOfWork.SaveChangesAsync();

            // Act - Try to retrieve with plain token (should fail)
            var result = await refreshTokenRepository.GetByTokenHashAsync(plainToken);

            // Assert
            Assert.Null(result); // Should not find it with plain token
        }

        [Fact]
        public void HashToken_ShouldProduceDeterministicHash()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtProvider>();
            var token = "test-token-123";

            // Act
            var hash1 = jwtProvider.HashToken(token);
            var hash2 = jwtProvider.HashToken(token);

            // Assert
            Assert.Equal(hash1, hash2); // Same input should produce same hash
        }

        [Fact]
        public void HashToken_ShouldProduceDifferentHashesForDifferentTokens()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtProvider>();

            // Act
            var hash1 = jwtProvider.HashToken("token1");
            var hash2 = jwtProvider.HashToken("token2");

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
