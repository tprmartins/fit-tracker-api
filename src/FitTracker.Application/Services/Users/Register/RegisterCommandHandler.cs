using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.RefreshTokens;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Errors;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Enums;

namespace FitTracker.Application.Services.Users.Register
{
    internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHash _passwordHash;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RegisterCommandHandler(
            IUserRepository userRepository, 
            IJwtProvider jwtProvider, 
            IUnitOfWork unitOfWork, 
            IPasswordHash passwordHash,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
            _passwordHash = passwordHash;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            var document = Document.Create(request.Document);

            if (document.IsFailure)
            {
                return Result.Failure<RegisterResponse>(document.Error);
            }    

            bool userExists = await _userRepository.IsUserDocumentAlreadyExists(document.Value, cancellationToken);

            if (userExists)
            {
                return Result.Failure<RegisterResponse>(DomainErrors.User.AlreadyExists);
            }

            var email = Email.Create(request.Email);

            if (email.IsFailure)
            {
                return Result.Failure<RegisterResponse>(email.Error);
            }

            bool emailExists = await _userRepository.IsUserEmailAlreadyExists(email.Value, cancellationToken);

            if (emailExists)
            {
                return Result.Failure<RegisterResponse>(DomainErrors.User.EmailAlreadyExists);
            }

            var passwordHash = _passwordHash.Hash(request.Password);

            User user = new(
                new UserId(Guid.NewGuid()), 
                email.Value, 
                document.Value, 
                request.Name, 
                passwordHash, 
                request.Phone,
                (UserRole)request.Role,
                UserStatus.Active);

            _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

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

            var response = new RegisterResponse(
                user.Id.Value, 
                accessToken, 
                refreshToken, 
                user.Document?.Value ?? string.Empty, 
                user.Name, 
                user.Email.Value, 
                DateTime.Now);

            return response;
        }
    }
}
