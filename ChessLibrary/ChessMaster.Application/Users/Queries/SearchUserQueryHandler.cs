namespace ChessMaster.Application.Users.Queries;

public class SearchUserQueryHandler: BaseHandler, IRequestHandler<SearchUserQuery, User>
{
    public SearchUserQueryHandler(ITenantFactory tenantFactory) : base(tenantFactory)
    {
    }
    
    public async Task<User> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var user = await GetTenant().Users.GetByUsername(request.Username, cancellationToken);
        return user;
    }
}