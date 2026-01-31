using FluentValidation;

namespace FitTracker.Application.Services.Users.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Document)
                .NotEmpty()
                .OverridePropertyName("DocumentOrEmail")
                .WithMessage("Document or Email is required");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long");
        }
    }
}
