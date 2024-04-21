namespace ChessMaster.Application.Games.Commands;

public class JoinGameCommandHandler: BaseHandler, IRequestHandler<JoinGameCommand, Game>
{
    public JoinGameCommandHandler(ITenantFactory tenantFactory) : 
        base(tenantFactory)
    {
    }
    
    public async Task<Game> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        // Business logic
        var tenantRepository = GetTenant();
        
        var game = await tenantRepository.Games.GetById(request.GameId, cancellationToken);
        
        game.Join(request.PlayerId);
        
        await tenantRepository.Games.Update(game, cancellationToken);
        await tenantRepository.CommitAsync(cancellationToken);
        
        return game;
    }
}