namespace ChessMaster.Application.CQRS.Games.Commands;

public class JoinGameCommandHandler: BaseHandler, IRequestHandler<JoinGameCommand, Game>
{
    public JoinGameCommandHandler(ITenantFactory tenantFactory) : 
        base(tenantFactory)
    {
    }
    
    public async Task<Game> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        // Validation
        ArgumentNullException.ThrowIfNull(request);
        
        // Business logic
        var tenant = GetTenant();
        
        var game = await tenant.Games.GetById(request.GameId, cancellationToken);
        
        game.Join(request.PlayerId);
        
        await tenant.Games.Update(game, cancellationToken);
        await tenant.CommitAsync(cancellationToken);
        
        return game;
    }
}