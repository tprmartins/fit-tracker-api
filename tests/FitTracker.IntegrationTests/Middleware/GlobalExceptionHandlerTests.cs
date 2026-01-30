using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace FitTracker.IntegrationTests.Middleware
{
    public class GlobalExceptionHandlerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GlobalExceptionHandlerTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task UnhandledException_ShouldReturnProblemDetails()
        {
            // Arrange - Call an endpoint that doesn't exist to trigger an error
            var invalidEndpoint = "/api/user/nonexistent-endpoint";

            // Act
            var response = await _client.GetAsync(invalidEndpoint);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
            var contentType = response.Content.Headers.ContentType?.MediaType;
            Assert.Equal("application/problem+json", contentType);
        }

        [Fact]
        public async Task Response_ShouldContainCorrelationId()
        {
            // Arrange
            var endpoint = "/api/user/login";
            var requestData = new { Document = "12345678901", Password = "test" };

            // Act
            var response = await _client.PostAsJsonAsync(endpoint, requestData);

            // Assert
            Assert.True(response.Headers.Contains("X-Correlation-Id"), 
                "Response should contain X-Correlation-Id header");
            
            var correlationId = response.Headers.GetValues("X-Correlation-Id").FirstOrDefault();
            Assert.False(string.IsNullOrEmpty(correlationId), 
                "X-Correlation-Id should not be empty");
        }
    }
}
