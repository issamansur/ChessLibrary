namespace ChessMaster.Application.Games.Queries;

public class GetGameQueryHandler: BaseHandler, IRequestHandler<GetGameQuery, Game>
{
    public GetGameQueryHandler(ITenantFactory tenantFactory):
        base(tenantFactory)
    {
        
    }

    public async Task<Game> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var game = await GetTenant().Games.GetById(request.GameId, cancellationToken);

        return game;
    }
}