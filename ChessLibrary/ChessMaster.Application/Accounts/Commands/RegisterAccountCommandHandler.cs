namespace ChessMaster.Application.Accounts.Commands;

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
        
        // Business logic
        var tenantRepository = GetTenant();

        if (await tenantRepository.Accounts.TryGetByEmail(request.Email, cancellationToken) != null)
        {
            throw new InvalidOperationException("Email already exists.");
        }
        
        if (await tenantRepository.Users.TryGetByUsername(request.Username, cancellationToken) != null)
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