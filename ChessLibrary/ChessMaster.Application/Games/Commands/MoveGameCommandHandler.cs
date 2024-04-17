namespace ChessMaster.Application.Games.Commands;

public class MoveGameCommandHandler: BaseHandler, IRequestHandler<MoveGameCommand, Game>
{
    public MoveGameCommandHandler(ITenantFactory tenantFactory) : 
        base(tenantFactory)
    {
        
    }
    
    public async Task<Game> Handle(MoveGameCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        // Business logic
        var tenantRepository = TenantRepository;
        
        var game = await tenantRepository.Games.GetById(request.GameId, cancellationToken);
        if (game == null)
        {
            throw new InvalidOperationException("Game does not exist.");
        }
        
        game.UpdateFEN();
    }
}