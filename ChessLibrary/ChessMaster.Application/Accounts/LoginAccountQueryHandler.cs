namespace ChessMaster.Application.Accounts;

public class LoginAccountQueryHandler: BaseHandler<Unit>, IRequestHandler<LoginAccountQuery, Unit>
{
    public LoginAccountQueryHandler(ITenantFactory tenantFactory)
        : base(tenantFactory)
    {
        
    }
    
    public async Task<Unit> Handle(LoginAccountQuery request, CancellationToken cancellationToken)
    {
        // Validation
        ValidateRequest(request);
        
        if (string.IsNullOrWhiteSpace(request.Login))
        {
            throw new ArgumentNullException(nameof(request.Login));
        }
        
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentNullException(nameof(request.Password));
        }
        
        // Business logic
        var tenantRepository = TenantRepository;
        
        var account = await tenantRepository.Accounts.GetByEmail(request.Login, cancellationToken);
        
        if (account == null)
        {
            throw new InvalidOperationException("Account not found.");
        }
        
        if (!account.VerifyByPassword(request.Password))
        {
            throw new InvalidOperationException("Invalid password.");
        }
        
        return Unit.Value;
    }
}