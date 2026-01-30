using FitTracker.Domain.Entities.Users;
using System.Security.Claims;

namespace FitTracker.Application.Abstractions
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        Guid GetTokenUser(string token);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        string HashToken(string token);
    }
}
