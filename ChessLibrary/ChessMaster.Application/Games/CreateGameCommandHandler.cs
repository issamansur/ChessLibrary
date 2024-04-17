namespace ChessMaster.Application.Games;

public class CreateGameCommandHandler: BaseHandler<Game>, IRequestHandler<CreateGameCommand, Game>
{
    public CreateGameCommandHandler(ITenantFactory tenantFactory): 
        base(tenantFactory)
    {
        
    }
    
    public async Task<Game> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        // Validation
        ValidateRequest(request);
        
        if (request.CreatorUserId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(request.CreatorUserId));
        }
        
        // Business logic
        var tenantRepository = TenantRepository;
        
        if (await tenantRepository.Users.GetById(request.CreatorUserId, cancellationToken) == null)
        {
            throw new InvalidOperationException("User does not exist.");
        }
        
        var game = Game.Create(request.CreatorUserId);
        
        await tenantRepository.Games.Create(game, cancellationToken);
        await tenantRepository.CommitAsync(cancellationToken);
        
        return game;
    }
}