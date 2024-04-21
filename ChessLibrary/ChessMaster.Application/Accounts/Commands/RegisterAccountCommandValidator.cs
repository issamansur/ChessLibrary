namespace ChessMaster.Application.Accounts.Commands;

public class RegisterAccountCommandValidator: AbstractValidator<RegisterAccountCommand>
{
    public RegisterAccountCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(6).MaximumLength(20).WithMessage("Username must be between 6 and 20 characters.")
            .Matches("^[a-zA-Z]+$").WithMessage("Username must contain only English letters");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Login must be a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");
    }
}