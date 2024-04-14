namespace ChessMaster.Application.Accounts;

public class CreateAccountCommandHandler : BaseHandler<Account>, IRequestHandler<RegisterAccountCommand, Account>
{
    public CreateAccountCommandHandler(ITenantRepository tenantRepository) :
        base(tenantRepository) { }

    public async Task<Account> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
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
        if (await _tenantRepository.Accounts.Exists(request.Email, cancellationToken))
        {
            throw new InvalidOperationException("Email already exists.");
        }
        
        if (await _tenantRepository.Users.Exists(request.Username, cancellationToken))
        {
            throw new InvalidOperationException("Username already exists.");
        }
        
        var user = User.Create(request.Username);
        var account = Account.Create(user, request.Email, request.Password);
        
        await _tenantRepository.Users.Create(user, cancellationToken);
        await _tenantRepository.Accounts.Register(account, cancellationToken);
        
        return account;
    }
}