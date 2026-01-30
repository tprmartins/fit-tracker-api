using FluentValidation;

namespace FitTracker.Application.Services.Users.Invite
{
    public class InviteStudentCommandValidator : AbstractValidator<InviteStudentCommand>
    {
        public InviteStudentCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Phone).MaximumLength(50);
        }
    }
}
