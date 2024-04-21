namespace ChessMaster.Application.Games.Queries;

public class SearchGameQueryHandler: BaseHandler, IRequestHandler<SearchGameQuery, IReadOnlyCollection<Game>>
{
    public SearchGameQueryHandler(ITenantFactory tenantFactory):
        base(tenantFactory)
    {
        
    }

    public async Task<IReadOnlyCollection<Game>> Handle(SearchGameQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var games = await GetTenant().Games.Search(request.Filter, cancellationToken);

        return games;
    }
}