using ChessMaster.Application.Services;

namespace ChessMaster.Application.CQRS.Accounts.Queries;

public class LoginAccountQueryHandler: BaseHandler, IRequestHandler<LoginAccountQuery, string>
{
    private IAuthService _authService;
    
    public LoginAccountQueryHandler(
        ITenantFactory tenantFactory,
        IAuthService authService
    ): 
        base(tenantFactory)
    {
        _authService = authService;
    }
    
    public async Task<string> Handle(LoginAccountQuery request, CancellationToken cancellationToken)
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
        
        var token = _authService.GenerateToken(account);

        return token;
    }
}