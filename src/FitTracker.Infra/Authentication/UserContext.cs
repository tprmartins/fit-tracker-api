using FitTracker.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FitTracker.Infra.Authentication
{
    internal sealed class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            }
        }

        public FitTracker.Domain.Enums.UserRole Role
        {
            get
            {
                var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

                if (Enum.TryParse<FitTracker.Domain.Enums.UserRole>(role, out var parsedRole))
                {
                    return parsedRole;
                }

                return 0; // Unknown or default
            }
        }
    }
}
