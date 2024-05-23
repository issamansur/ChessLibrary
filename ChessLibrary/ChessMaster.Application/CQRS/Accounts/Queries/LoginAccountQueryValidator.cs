namespace ChessMaster.Application.CQRS.Accounts.Queries;

public class LoginAccountQueryValidator: AbstractValidator<LoginAccountQuery>
{
    public LoginAccountQueryValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login is required.")
            .EmailAddress().WithMessage("Login must be a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}