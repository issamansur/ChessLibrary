namespace ChessMaster.Application.CQRS.Accounts.Queries;

public class LoginAccountQueryHandler: BaseHandler, IRequestHandler<LoginAccountQuery>
{
    public LoginAccountQueryHandler(ITenantFactory tenantFactory)
        : base(tenantFactory)
    {
        
    }
    
    public async Task Handle(LoginAccountQuery request, CancellationToken cancellationToken)
    {
        // Validation
        ArgumentNullException.ThrowIfNull(request);
        
        // Business logic
        var tenant = GetTenant();
        
        var account = await tenant.Accounts.GetByEmail(request.Login, cancellationToken);
        
        if (account == null)
        {
            throw new InvalidOperationException("Account not found.");
        }
        
        if (!account.VerifyByPassword(request.Password))
        {
            throw new InvalidOperationException("Invalid password.");
        }
    }
}