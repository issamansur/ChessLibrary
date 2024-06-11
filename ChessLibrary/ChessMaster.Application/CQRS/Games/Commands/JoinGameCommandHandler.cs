using ChessMaster.Application.Services;

namespace ChessMaster.Application.CQRS.Games.Commands;

// See actor service in Controllers
/*
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
        var game = await _actorService.Ask<Game>(request);
        
        return game;
    }
}
*/