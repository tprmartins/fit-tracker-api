using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Users.Login;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Users.GetByRole
{
    public sealed class GetUsersByRoleQueryHandler : IQueryHandler<GetUsersByRoleQuery, List<UserResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersByRoleQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<List<UserResponse>>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByRoleAsync((UserRole)request.Role, cancellationToken);

            var response = users.Select(u => new UserResponse(
                u.Name,
                u.Id.Value.ToString(),
                u.Document?.Value ?? string.Empty,
                u.Email.Value,
                u.Phone,
                (int)u.Role,
                (int)u.Status
            )).ToList();

            return response;
        }
    }
}
