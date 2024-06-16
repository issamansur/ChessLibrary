using ChessMaster.Application.DTOs;
using ChessMaster.Application.Services;

namespace ChessMaster.Application.CQRS.Accounts.Queries;

public class LoginAccountQueryHandler: BaseHandler, IRequestHandler<LoginAccountQuery, LoginResult>
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
    
    public async Task<LoginResult> Handle(LoginAccountQuery request, CancellationToken cancellationToken)
    {
        // Validation
        ArgumentNullException.ThrowIfNull(request);
        
        // Business logic
        var tenant = GetTenant();

        Account? account = null;
        User? user = null;
        
        if (request.Login.Contains('@'))
        {
            account = await tenant.Accounts.TryGetByEmail(request.Login, cancellationToken);
        } 
        else
        {
            user = await tenant.Users.TryGetByUsername(request.Login, cancellationToken);
            if (user != null)
            {
                account = await tenant.Accounts.GetByIdAsync(user.Id, cancellationToken);
            }
        }
        
        if (account == null)
        {
            throw new InvalidOperationException("Account not found.");
        }
        
        if (!account.VerifyByPassword(request.Password))
        {
            throw new InvalidOperationException("Invalid password.");
        }
        
        if (user == null)
        {
            user = await tenant.Users.GetByIdAsync(account.UserId, cancellationToken);
        }
        
        var token = _authService.GenerateToken(account);

        return new LoginResult(user, token);
    }
}