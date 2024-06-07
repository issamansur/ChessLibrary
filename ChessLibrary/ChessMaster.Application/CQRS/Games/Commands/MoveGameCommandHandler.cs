using ChessMaster.Application.Services;

namespace ChessMaster.Application.CQRS.Games.Commands;

public class MoveGameCommandHandler: BaseHandler, IRequestHandler<MoveGameCommand, Game>
{
    IActorService _actorService;
    public MoveGameCommandHandler(ITenantFactory tenantFactory, IActorService actorService) : 
        base(tenantFactory)
    {
        _actorService = actorService;
    }
    
    public async Task<Game> Handle(MoveGameCommand request, CancellationToken cancellationToken)
    {
        // Validation
        ArgumentNullException.ThrowIfNull(request);
        
        // Business logic
        var tenant = GetTenant();
        
        var game = await tenant.Games.GetById(request.GameId, cancellationToken);
        if (game == null)
        {
            throw new InvalidOperationException("Game does not exist.");
        }
        
        string newFen = await _actorService.Ask<string>(request);
        Console.WriteLine($"New FEN: {newFen}");
        game.UpdateFEN(newFen);
        
        await tenant.Games.Update(game, cancellationToken);
        await tenant.CommitAsync(cancellationToken);

        return game;
    }
}