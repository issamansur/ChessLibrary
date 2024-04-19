namespace ChessMaster.Application.Users.Queries;

public class GetUserQueryHandler: BaseHandler, IRequestHandler<GetUserQuery, User>
{
    public GetUserQueryHandler(ITenantFactory tenantFactory) : base(tenantFactory)
    {
    }
    
    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var user = await GetTenant().Users.GetById(request.Id, cancellationToken);
        return user;
    }
}