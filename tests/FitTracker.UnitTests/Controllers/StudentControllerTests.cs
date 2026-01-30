using FitTracker.Api.Controllers;
using FitTracker.Application.Services.Users.GetByRole;
using FitTracker.Application.Services.Users.Login;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Shared;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FitTracker.UnitTests.Controllers
{
    public class StudentControllerTests
    {
        private readonly Mock<ISender> _senderMock;
        private readonly StudentController _controller;

        public StudentControllerTests()
        {
            _senderMock = new Mock<ISender>();
            _controller = new StudentController(_senderMock.Object);
        }

        [Fact]
        public async Task GetAllStudents_ShouldReturnOk_WhenSuccess()
        {
            // Arrange
            var students = new List<UserResponse>
            {
                new UserResponse("Student 1", Guid.NewGuid().ToString(), "12345678901", "student1@test.com", "123456789")
            };
            var result = Result.Success(students);

            _senderMock
                .Setup(s => s.Send(It.IsAny<GetUsersByRoleQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            // Act
            var actionResult = await _controller.GetAllStudents(CancellationToken.None);

            // Assert
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(students);
            
            _senderMock.Verify(
                s => s.Send(
                    It.Is<GetUsersByRoleQuery>(q => q.Role == (int)UserRole.Student),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllStudents_ShouldReturnBadRequest_WhenFailure()
        {
            // Arrange
            var error = new Error("Test.Error", "Test error message");
            var result = Result.Failure<List<UserResponse>>(error);

            _senderMock
                .Setup(s => s.Send(It.IsAny<GetUsersByRoleQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            // Act
            var actionResult = await _controller.GetAllStudents(CancellationToken.None);

            // Assert
            var badRequestResult = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
            var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Subject;
            problemDetails.Type.Should().Be(error.Code);
            problemDetails.Detail.Should().Be(error.Message);
        }
    }
}
