using FluentValidation;

namespace FitTracker.Application.Services.Users.CompleteRegistration
{
    public class CompleteRegistrationCommandValidator : AbstractValidator<CompleteRegistrationCommand>
    {
        public CompleteRegistrationCommandValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.Document).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
