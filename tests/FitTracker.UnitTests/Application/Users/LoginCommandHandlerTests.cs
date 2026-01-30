using FitTracker.Application.Abstractions;
using FitTracker.Application.Services.Users.Login;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using FitTracker.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace FitTracker.UnitTests.Application.Users
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtProvider> _jwtProviderMock;
        private readonly Mock<IPasswordHash> _passwordHashMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtProviderMock = new Mock<IJwtProvider>();
            _passwordHashMock = new Mock<IPasswordHash>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new LoginCommandHandler(
                _userRepositoryMock.Object,
                _jwtProviderMock.Object,
                _passwordHashMock.Object,
                _refreshTokenRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnLoginResponse_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new LoginCommand("test@test.com", "password123");
            var user = new User(
                new UserId(Guid.NewGuid()),
                Email.Create("test@test.com").Value,
                Document.Create("12345678901").Value,
                "Test User",
                "hashed_password",
                "123456789",
                UserRole.Student,
                UserStatus.Active);

            _userRepositoryMock
                .Setup(u => u.GetByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordHashMock
                .Setup(p => p.Verify(user.Password!, command.Password))
                .Returns(true);

            _jwtProviderMock
                .Setup(j => j.GenerateAccessToken(user))
                .Returns("access_token");

            _jwtProviderMock
                .Setup(j => j.GenerateRefreshToken())
                .Returns("refresh_token");

            _jwtProviderMock
                .Setup(j => j.HashToken("refresh_token"))
                .Returns("hashed_refresh_token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.AccessToken.Should().Be("access_token");
            result.Value.RefreshToken.Should().Be("refresh_token");
            
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var command = new LoginCommand("nonexistent@test.com", "password123");

            _userRepositoryMock
                .Setup(u => u.GetByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("User.InvalidCredentials");
        }
    }
}
