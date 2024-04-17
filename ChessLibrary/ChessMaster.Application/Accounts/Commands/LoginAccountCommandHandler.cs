namespace ChessMaster.Application.Accounts.Commands;

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