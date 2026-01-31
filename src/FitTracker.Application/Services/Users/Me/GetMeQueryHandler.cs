using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Users.Login;
using FitTracker.Domain.Errors;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Entities.Users;

namespace FitTracker.Application.Services.Users.Me
{
    internal sealed class GetMeQueryHandler : IQueryHandler<GetMeQuery, UserResponse>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public GetMeQueryHandler(IUserContext userContext, IUserRepository userRepository)
        {
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<Result<UserResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContext.UserId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<UserResponse>(DomainErrors.User.NotFound);
            }

            return new UserResponse(user.Name, user.Id.Value.ToString(), user.Document?.Value ?? "", user.Email.Value, user.Phone, (int)user.Role, (int)user.Status);
        }
    }
}
