namespace ChessMaster.Application.Games;

public class JoinGameCommandHandler: BaseHandler<Game>, IRequestHandler<JoinGameCommand, Game>
{
    public JoinGameCommandHandler(ITenantFactory tenantFactory) : 
        base(tenantFactory)
    {
    }
    
    public async Task<Game> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
        ValidateRequest(request);
        
        // Business logic
        var tenantRepository = TenantRepository;
        
        var game = await tenantRepository.Games.GetById(request.GameId, cancellationToken);
        if (game == null)
        {
            throw new InvalidOperationException("Game does not exist.");
        }
        
        game.Join(request.PlayerId);
        
        await tenantRepository.Games.Update(game, cancellationToken);
        await tenantRepository.CommitAsync(cancellationToken);
        
        return game;
    }
}