using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.RefreshTokens;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Errors;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using System.Security.Claims;

namespace FitTracker.Application.Services.Users.RefreshTokens
{
    internal sealed class RefreshTokensCommandHandler : ICommandHandler<RefreshTokensCommand, RefreshTokensResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokensCommandHandler(
            IUserRepository userRepository, 
            IJwtProvider jwtProvider,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RefreshTokensResponse>> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
        {
            var principal = _jwtProvider.GetPrincipalFromExpiredToken(request.Token);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Result.Failure<RefreshTokensResponse>(DomainErrors.User.Invalid);
            }

            string refreshTokenHash = _jwtProvider.HashToken(request.ProvidedRefreshToken);
            var refreshTokenEntity = await _refreshTokenRepository.GetByTokenHashAsync(refreshTokenHash, cancellationToken);

            if (refreshTokenEntity is null || !refreshTokenEntity.IsActive || refreshTokenEntity.UserId.Value != userId)
            {
                return Result.Failure<RefreshTokensResponse>(DomainErrors.User.Invalid);
            }

            User? user = await _userRepository.GetByIdAsync(new UserId(userId), cancellationToken);

            if (user is null)
            {
                return Result.Failure<RefreshTokensResponse>(DomainErrors.User.NotFound);
            }

            // Rotação: Usar o token antigo e gerar um novo
            refreshTokenEntity.Use();
            _refreshTokenRepository.Update(refreshTokenEntity);

            string newAccessToken = _jwtProvider.GenerateAccessToken(user);
            string newRefreshToken = _jwtProvider.GenerateRefreshToken();
            string newRefreshTokenHash = _jwtProvider.HashToken(newRefreshToken);

            var newRefreshTokenEntity = new RefreshToken(
                Guid.NewGuid(),
                user.Id,
                newRefreshTokenHash,
                DateTime.UtcNow.AddDays(7));

            _refreshTokenRepository.Add(newRefreshTokenEntity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RefreshTokensResponse(newAccessToken, newRefreshToken);
        }
    }
}
