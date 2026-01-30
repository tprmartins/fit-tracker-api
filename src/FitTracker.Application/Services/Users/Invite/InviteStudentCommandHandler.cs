using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Errors;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using FitTracker.Domain.ValueObjects;

namespace FitTracker.Application.Services.Users.Invite
{
    internal sealed class InviteStudentCommandHandler : ICommandHandler<InviteStudentCommand, InviteStudentResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public InviteStudentCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Result<InviteStudentResponse>> Handle(InviteStudentCommand request, CancellationToken cancellationToken)
        {
            var email = Email.Create(request.Email);

            if (email.IsFailure)
            {
                return Result.Failure<InviteStudentResponse>(email.Error);
            }

            bool emailExists = await _userRepository.IsUserEmailAlreadyExists(email.Value, cancellationToken);

            if (emailExists)
            {
                return Result.Failure<InviteStudentResponse>(DomainErrors.User.EmailAlreadyExists);
            }

            var user = User.InviteStudent(email.Value, request.Name, request.Phone);

            _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailService.SendInvitationEmailAsync(user.Email.Value, user.Name, user.RegistrationToken!, cancellationToken);

            return new InviteStudentResponse(user.Id.Value, user.Email.Value, user.Name, user.RegistrationToken!);
        }
    }
}
