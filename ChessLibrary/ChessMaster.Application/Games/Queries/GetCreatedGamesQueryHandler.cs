namespace ChessMaster.Application.Games.Queries;

public class GetCreatedGamesQueryHandler: BaseHandler, IRequestHandler<GetCreatedGamesQuery, IReadOnlyCollection<Game>>
{
    public GetCreatedGamesQueryHandler(ITenantFactory tenantFactory):
        base(tenantFactory)
    {
        
    }

    public async Task<IReadOnlyCollection<Game>> Handle(GetCreatedGamesQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var games = await GetTenant().Games.GetCreatedGames(cancellationToken);

        return games;
    }
}