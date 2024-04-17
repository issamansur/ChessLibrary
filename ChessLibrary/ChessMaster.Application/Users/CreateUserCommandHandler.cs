namespace ChessMaster.Application.Users;

public class CreateUserCommandHandler: BaseHandler<User>, IRequestHandler<CreateUserCommand, User>
{
    public CreateUserCommandHandler(ITenantFactory tenantFactory):
        base(tenantFactory)
    {
        
    }
    
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validation
        ValidateRequest(request);
        
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            throw new ArgumentNullException(nameof(request.Username));
        }
        
        // Business logic
        var tenantRepository = TenantRepository;
        
        if (await tenantRepository.Users.Exists(request.Username, cancellationToken))
        {
            throw new InvalidOperationException("Username already exists.");
        }
        
        var user = User.Create(request.Username);
        
        await tenantRepository.Users.Create(user, cancellationToken);
        await tenantRepository.CommitAsync(cancellationToken);
        
        return user;
    }
}