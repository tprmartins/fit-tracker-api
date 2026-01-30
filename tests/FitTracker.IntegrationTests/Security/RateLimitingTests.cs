using System.Net;
using System.Net.Http.Json;
using FitTracker.Application.Services.Users.Login;
using FitTracker.Application.Services.Users.Register;
using Xunit;

namespace FitTracker.IntegrationTests.Security
{
    public class RateLimitingTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public RateLimitingTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ShouldReturnTooManyRequests_WhenRateLimitExceeded()
        {
            // Arrange
            var loginRequest = new LoginRequest("12345678901", "", "password123");
            var successfulRequests = 0;
            var rateLimitedRequests = 0;

            // Act - Make 10 requests (limit is 5 per minute)
            for (int i = 0; i < 10; i++)
            {
                var response = await _client.PostAsJsonAsync("/api/user/login", loginRequest);
                
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    rateLimitedRequests++;
                }
                else
                {
                    successfulRequests++;
                }
            }

            // Assert
            Assert.True(rateLimitedRequests > 0, "Expected some requests to be rate limited");
            Assert.True(successfulRequests <= 7, "Expected no more than 7 successful requests (5 limit + 2 queue)");
        }
    }
}
