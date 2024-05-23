namespace ChessMaster.Application.CQRS.Games.Queries;

public class SearchGameQueryHandler: BaseHandler, IRequestHandler<SearchGameQuery, IReadOnlyCollection<Game>>
{
    public SearchGameQueryHandler(ITenantFactory tenantFactory):
        base(tenantFactory)
    {
        
    }

    public async Task<IReadOnlyCollection<Game>> Handle(SearchGameQuery request, CancellationToken cancellationToken)
    {
        // Validation
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        // Business logic
        var tenant = GetTenant();
        
        var games = await tenant.Games.Search(request.Filter, cancellationToken);

        return games;
    }
}