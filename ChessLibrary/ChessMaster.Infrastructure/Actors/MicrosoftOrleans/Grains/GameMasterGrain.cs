using System.Threading.Channels;
using ChessMaster.ChessLibrary;
using ChessMaster.Infrastructure.Actors.Common;
using ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Common;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Grains;

public class GameMasterGrain: Grain, IGameMasterGrain
{
    public Guid GameId { get; private init; }
    public Game GameFromDB { get; private set; }
    public Chess Game { get; private set; }
    
    private bool _isFirstTime = true;
    
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public GameMasterGrain(Guid gameId, IServiceScopeFactory serviceScopeFactory)
    {
        GameId = gameId;
        
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    // Override methods
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (_isFirstTime)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var tenant = scope.ServiceProvider.GetRequiredService<ITenantFactory>().GetRepository();
                // Load FEN from DB if exists
                GameFromDB = await tenant.Games.GetByIdAsync(GameId, cancellationToken);
                Game = GameFromDB.Fen.ToChess();
                
                // Set flag to false
                _isFirstTime = false;
            }
        }
        
        await base.OnActivateAsync(cancellationToken);
    }
    
    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var tenant = scope.ServiceProvider.GetRequiredService<ITenantFactory>().GetRepository();
            
            // UpdateAsync FEN in DB
            var currentFen = Game.GetFen();
            GameFromDB.UpdateFEN(currentFen);
            await tenant.Games.UpdateAsync(GameFromDB, cancellationToken);
            await tenant.CommitAsync(cancellationToken);
        }

        await base.OnDeactivateAsync(reason, cancellationToken);
    }
    
    public async Task<string> Move(Guid playerId, Guid gameId, string move)
    {
        Game.Move(move);
        return Game.GetFen();
    }
}