namespace ChessMaster.Application.CQRS.Users.Queries;

public class GetUserByUsernameQueryHandler: BaseHandler, IRequestHandler<GetUserByUsernameQuery, User>
{
    public GetUserByUsernameQueryHandler(ITenantFactory tenantFactory) : base(tenantFactory)
    {
    }
    
    public async Task<User> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        // Validation
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        // Business logic
        var tenant = GetTenant();
        
        var user = await tenant.Users.GetByUsername(request.Username, cancellationToken);
        return user;
    }
}