namespace ChessMaster.Application.CQRS.Accounts.Commands;

public class RegisterAccountCommandHandler : BaseHandler, IRequestHandler<RegisterAccountCommand>
{
    public RegisterAccountCommandHandler(ITenantFactory tenantFactory) : 
        base(tenantFactory)
    {
        
    }

    public async Task Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        // Validation
        ArgumentNullException.ThrowIfNull(request);
        
        // Business logic
        var tenant = GetTenant();

        if (await tenant.Accounts.TryGetByEmail(request.Email, cancellationToken) != null)
        {
            throw new InvalidOperationException("Email already exists.");
        }
        
        if (await tenant.Users.TryGetByUsername(request.Username, cancellationToken) != null)
        {
            throw new InvalidOperationException("Username already exists.");
        }
        
        var user = User.Create(request.Username);
        var account = Account.Create(user, request.Email, request.Password);

        await tenant.Users.Create(user, cancellationToken);
        await tenant.Accounts.Create(account, cancellationToken);
        await tenant.CommitAsync(cancellationToken);
    }
}