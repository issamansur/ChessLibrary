namespace ChessMaster.Application.Users.Queries;

public class SearchUserQueryHandler: BaseHandler, IRequestHandler<SearchUserQuery, IReadOnlyCollection<User>>
{
    public SearchUserQueryHandler(ITenantFactory tenantFactory) : base(tenantFactory)
    {
    }
    
    public async Task<IReadOnlyCollection<User>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var users = await GetTenant().Users.Search(request.Query, cancellationToken);
        return users;
    }
}