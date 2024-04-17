namespace ChessMaster.Application.Accounts;

public class RegisterAccountCommandHandler : BaseHandler, IRequestHandler<RegisterAccountCommand>
{
    public RegisterAccountCommandHandler(ITenantFactory tenantFactory) : 
        base(tenantFactory)
    {
        
    }

    public async Task Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        // Validation
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            throw new ArgumentNullException(nameof(request.Username));
        }
        
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentNullException(nameof(request.Email));
        }
        
        // Business logic
        var tenantRepository = TenantRepository;

        if (await tenantRepository.Accounts.Exists(request.Email, cancellationToken))
        {
            throw new InvalidOperationException("Email already exists.");
        }
        
        if (await tenantRepository.Users.Exists(request.Username, cancellationToken))
        {
            throw new InvalidOperationException("Username already exists.");
        }
        
        var user = User.Create(request.Username);
        var account = Account.Create(user, request.Email, request.Password);

        await tenantRepository.Users.Create(user, cancellationToken);
        await tenantRepository.Accounts.Create(account, cancellationToken);
        await tenantRepository.CommitAsync(cancellationToken);
    }
}