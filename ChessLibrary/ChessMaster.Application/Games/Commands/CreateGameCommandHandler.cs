namespace ChessMaster.Application.Games.Commands;

public class CreateGameCommandHandler: BaseHandler, IRequestHandler<CreateGameCommand, Game>
{
    public CreateGameCommandHandler(ITenantFactory tenantFactory): 
        base(tenantFactory)
    {
        
    }
    
    public async Task<Game> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        // Validation
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        // Business logic
        var tenantRepository = GetTenant();

        await tenantRepository.Users.GetById(request.CreatorUserId, cancellationToken);
        
        var game = Game.Create(request.CreatorUserId);
        
        await tenantRepository.Games.Create(game, cancellationToken);
        await tenantRepository.CommitAsync(cancellationToken);
        
        return game;
    }
}