using FitTracker.Application.Abstractions;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Repositories;
using MediatR;

namespace FitTracker.Application.Services.Users.UnblockUser;

public class UnblockUserCommandHandler : IRequestHandler<UnblockUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UnblockUserCommandHandler(IUserRepository userRepository, IUserContext userContext, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
    {
        // Verify that current user is Admin
        if (_userContext.Role != UserRole.Admin)
        {
            throw new UnauthorizedAccessException("Only administrators can unblock users");
        }

        var userToUnblock = await _userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
        if (userToUnblock == null)
        {
            throw new InvalidOperationException("User not found");
        }

        userToUnblock.Unblock();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
