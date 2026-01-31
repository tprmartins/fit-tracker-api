using FitTracker.Application.Abstractions;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Repositories;
using MediatR;

namespace FitTracker.Application.Services.Users.BlockUser;

public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public BlockUserCommandHandler(IUserRepository userRepository, IUserContext userContext, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        // Verify that current user is Admin
        if (_userContext.Role != UserRole.Admin)
        {
            throw new UnauthorizedAccessException("Only administrators can block users");
        }

        var userToBlock = await _userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
        if (userToBlock == null)
        {
            throw new InvalidOperationException("User not found");
        }

        // Prevent blocking yourself
        if (userToBlock.Id.Value == _userContext.UserId)
        {
            throw new InvalidOperationException("You cannot block yourself");
        }

        userToBlock.Block(new UserId(_userContext.UserId));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
