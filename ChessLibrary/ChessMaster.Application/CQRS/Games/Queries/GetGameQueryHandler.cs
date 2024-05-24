namespace ChessMaster.Application.CQRS.Games.Queries;

public class GetGameQueryHandler: BaseHandler, IRequestHandler<GetGameQuery, Game>
{
    public GetGameQueryHandler(ITenantFactory tenantFactory):
        base(tenantFactory)
    {
        
    }

    public async Task<Game> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        // Validation
        ArgumentNullException.ThrowIfNull(request);
        
        // Business logic
        var tenant = GetTenant();
        
        var game = await tenant.Games.GetById(request.GameId, cancellationToken);

        return game;
    }
}