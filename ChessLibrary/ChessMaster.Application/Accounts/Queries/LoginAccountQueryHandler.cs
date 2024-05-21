namespace ChessMaster.Application.Accounts.Queries;

public class LoginAccountQueryHandler: BaseHandler, IRequestHandler<LoginAccountQuery>
{
    public LoginAccountQueryHandler(ITenantFactory tenantFactory)
        : base(tenantFactory)
    {
        
    }
    
    public async Task Handle(LoginAccountQuery request, CancellationToken cancellationToken)
    {
        // Validation
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        // Business logic
        var tenantRepository = GetTenant();
        
        var account = await tenantRepository.Accounts.GetByEmail(request.Login, cancellationToken);
        
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