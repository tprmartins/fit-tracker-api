using FitTracker.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infra.BackgroundJobs
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly ILogger<TokenCleanupService> _logger;
        private readonly IServiceScopeFactory _serviceProviderFactory;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(24);

        public TokenCleanupService(
            ILogger<TokenCleanupService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _serviceProviderFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Token Cleanup Service is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupExpiredTokensAsync(stoppingToken);
                    await Task.Delay(_cleanupInterval, stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Error occurred while cleaning up tokens");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }

            _logger.LogInformation("Token Cleanup Service is stopping");
        }

        private async Task CleanupExpiredTokensAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProviderFactory.CreateScope();
            var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var tokensToRemove = await refreshTokenRepository.GetAllExpiredOrUsedTokensAsync(cancellationToken);

            if (tokensToRemove.Any())
            {
                refreshTokenRepository.RemoveRange(tokensToRemove);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("Cleaned up {Count} expired/used tokens", tokensToRemove.Count());
            }
        }
    }
}
