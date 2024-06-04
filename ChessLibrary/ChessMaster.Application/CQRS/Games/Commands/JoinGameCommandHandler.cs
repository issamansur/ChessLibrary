using ChessMaster.Application.Services;

namespace ChessMaster.Application.CQRS.Games.Commands;

public class JoinGameCommandHandler: BaseHandler, IRequestHandler<JoinGameCommand, Game>
{
    IActorService _actorService;
    
    public JoinGameCommandHandler(ITenantFactory tenantFactory, IActorService actorService) : 
        base(tenantFactory)
    {
        _actorService = actorService;
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
        
        // If all goes well, tell the actor service to join the game
        _actorService.Tell(request);
        
        return game;
    }
}