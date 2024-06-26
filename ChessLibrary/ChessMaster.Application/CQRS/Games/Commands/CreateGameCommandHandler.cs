namespace ChessMaster.Application.CQRS.Games.Commands;

public class CreateGameCommandHandler: BaseHandler, IRequestHandler<CreateGameCommand, Game>
{
    public CreateGameCommandHandler(ITenantFactory tenantFactory): 
        base(tenantFactory)
    {
        
    }
    
    public async Task<Game> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        // Validation
        ArgumentNullException.ThrowIfNull(request);
        
        // Business logic
        var tenant = GetTenant();

        await tenant.Users.GetByIdAsync(request.CreatorUserId, cancellationToken);
        
        var game = Game.Create(request.CreatorUserId);
        
        await tenant.Games.CreateAsync(game, cancellationToken);
        await tenant.CommitAsync(cancellationToken);
        
        return game;
    }
}