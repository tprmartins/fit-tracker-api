using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Users
{
    internal sealed class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserAdminResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public GetAllUsersQueryHandler(IUserRepository userRepository, IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<Result<IEnumerable<UserAdminResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            if (_userContext.Role != UserRole.Admin)
            {
                return Result.Failure<IEnumerable<UserAdminResponse>>(new Error("Auth.Unauthorized", "Only admins can view all users."));
            }

            var users = await _userRepository.GetAllAsync(cancellationToken);

            var response = users.Select(user => new UserAdminResponse(
                user.Id.Value,
                user.Name,
                user.Email.Value,
                user.Document?.Value ?? string.Empty,
                user.Phone,
                (int)user.Role,
                user.IsActive,
                user.BlockedAt
            ));

            return Result.Success(response);
        }
    }
}
