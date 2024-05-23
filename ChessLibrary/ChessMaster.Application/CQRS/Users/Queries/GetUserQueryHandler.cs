namespace ChessMaster.Application.CQRS.Users.Queries;

public class GetUserQueryHandler: BaseHandler, IRequestHandler<GetUserQuery, User>
{
    public GetUserQueryHandler(ITenantFactory tenantFactory) : base(tenantFactory)
    {
    }
    
    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        // Validation
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        // Business logic
        var tenant = GetTenant();
        
        var user = await tenant.Users.GetById(request.Id, cancellationToken);
        
        return user;
    }
}