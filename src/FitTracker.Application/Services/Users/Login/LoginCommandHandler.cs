using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.RefreshTokens;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Errors;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using FitTracker.Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace FitTracker.Application.Services.Users.Login
{
    public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordHash _passwordHash;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(
            IUserRepository userRepository, 
            IJwtProvider jwtProvider, 
            IPasswordHash passwordHash,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHash = passwordHash;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user;
            
            string emailRegex = @"^[\w.-]+@(?=[a-z\d][^.]*\.)[a-z\d.-]*[^.]$";

            var isEmail = Regex.IsMatch(request.Document!, emailRegex!, RegexOptions.IgnoreCase);

            if (isEmail)
            {
                var email = Email.Create(request.Document!);                

                if (email.IsFailure)
                {
                    return Result.Failure<LoginResponse>(email.Error);
                }

                user = await _userRepository.GetByEmailAsync(email.Value, cancellationToken);
            }

            else
            {
                var document = Document.Create(request.Document!);

                if (document.IsFailure)
                {
                    return Result.Failure<LoginResponse>(document.Error);
                }

                user = await _userRepository.GetByDocumentAsync(document.Value, cancellationToken);
            }

            if (user is null || user.Password is null)
            {
                return Result.Failure<LoginResponse>(DomainErrors.User.InvalidCredentials);
            }

            bool isCredentialsValid = _passwordHash.Verify(user.Password, request.Password);

            if (!isCredentialsValid)
            {
                return Result.Failure<LoginResponse>(DomainErrors.User.InvalidCredentials);
            }

            string accessToken = _jwtProvider.GenerateAccessToken(user);

            string refreshToken = _jwtProvider.GenerateRefreshToken();
            string refreshTokenHash = _jwtProvider.HashToken(refreshToken);

            var refreshTokenEntity = new RefreshToken(
                Guid.NewGuid(),
                user.Id,
                refreshTokenHash,
                DateTime.UtcNow.AddDays(7));

            _refreshTokenRepository.Add(refreshTokenEntity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            UserResponse userResponse = new(
                user.Name, 
                user.Id.Value.ToString(), 
                user.Document?.Value ?? string.Empty, 
                user.Email.Value, 
                user.Phone);

            var response = new LoginResponse(accessToken, refreshToken);

            return response;
        }
    }
}
