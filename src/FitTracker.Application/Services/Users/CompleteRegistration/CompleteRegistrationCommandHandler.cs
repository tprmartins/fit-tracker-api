using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Errors;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using FitTracker.Domain.ValueObjects;

namespace FitTracker.Application.Services.Users.CompleteRegistration
{
    internal sealed class CompleteRegistrationCommandHandler : ICommandHandler<CompleteRegistrationCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHash _passwordHash;

        public CompleteRegistrationCommandHandler(
            IUserRepository userRepository, 
            IUnitOfWork unitOfWork, 
            IPasswordHash passwordHash)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHash = passwordHash;
        }

        public async Task<Result> Handle(CompleteRegistrationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByRegistrationTokenAsync(request.Token, cancellationToken);

            if (user is null)
            {
                return Result.Failure(DomainErrors.User.NotFound);
            }

            var documentResult = Document.Create(request.Document);
            if (documentResult.IsFailure)
            {
                return Result.Failure(documentResult.Error);
            }

            var passwordHash = _passwordHash.Hash(request.Password);

            var completionResult = user.CompleteRegistration(passwordHash, documentResult.Value);

            if (completionResult.IsFailure)
            {
                return Result.Failure(completionResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
