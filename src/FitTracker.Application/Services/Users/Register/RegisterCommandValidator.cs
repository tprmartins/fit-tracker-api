using FluentValidation;

namespace FitTracker.Application.Services.Users.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Document)
                .NotEmpty()
                .WithMessage("Document is required");

            RuleFor(x => x.Password)
                .NotEmpty()                
                .WithMessage("Password is required")
                .MaximumLength(256)
                .WithMessage("Password cannot be longer than 256 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .MaximumLength(128)
                .WithMessage("Email cannot be longer than 128 characters");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(128)
                .WithMessage("Name cannot be longer the 128 characters");

            RuleFor(x => x.Phone)
                .MaximumLength(50)
                .WithMessage("Phone cannot be longer than 50 characters");
        }
    }
}
