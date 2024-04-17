namespace ChessMaster.Application.Accounts;

public class LoginAccountCommandHandler: BaseHandler, IRequestHandler<LoginAccountCommand>
{
    public LoginAccountCommandHandler(ITenantFactory tenantFactory)
        : base(tenantFactory)
    {
        
    }
    
    public async Task Handle(LoginAccountCommand request, CancellationToken cancellationToken)
    {
        // Validation
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
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
    }
}