namespace ChessMaster.Application.Users.Queries;

public class GetUserByUsernameQueryHandler: BaseHandler, IRequestHandler<GetUserByUsernameQuery, User>
{
    public GetUserByUsernameQueryHandler(ITenantFactory tenantFactory) : base(tenantFactory)
    {
    }
    
    public async Task<User> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var user = await GetTenant().Users.GetByUsername(request.Username, cancellationToken);
        return user;
    }
}