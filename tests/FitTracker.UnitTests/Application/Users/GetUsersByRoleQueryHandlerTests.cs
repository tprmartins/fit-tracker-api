using FitTracker.Application.Services.Users.GetByRole;
using FitTracker.Application.Services.Users.Login;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace FitTracker.UnitTests.Application.Users
{
    public class GetUsersByRoleQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUsersByRoleQueryHandler _handler;

        public GetUsersByRoleQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetUsersByRoleQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUserList_WhenUsersExist()
        {
            // Arrange
            var role = UserRole.Student;
            var query = new GetUsersByRoleQuery((int)role);
            var users = new List<User>
            {
                new User(
                    new UserId(Guid.NewGuid()),
                    Email.Create("student@test.com").Value,
                    Document.Create("12345678901").Value,
                    "Student Name",
                    "hashed_password",
                    "123456789",
                    role,
                    UserStatus.Active)
            };

            _userRepositoryMock
                .Setup(u => u.GetByRoleAsync(role, It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value[0].Name.Should().Be("Student Name");
            result.Value[0].Email.Should().Be("student@test.com");
            result.Value[0].Document.Should().Be("12345678901");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var role = UserRole.Student;
            var query = new GetUsersByRoleQuery((int)role);

            _userRepositoryMock
                .Setup(u => u.GetByRoleAsync(role, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
        }
    }
}
