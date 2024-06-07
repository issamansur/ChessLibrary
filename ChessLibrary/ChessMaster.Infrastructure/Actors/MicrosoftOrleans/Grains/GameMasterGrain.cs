using System.Threading.Channels;
using ChessMaster.ChessModels;
using ChessMaster.ChessModels.Utils;
using ChessMaster.Infrastructure.Actors.Common;
using ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Common;

using Orleans;

namespace ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Grains;

public class GameMasterGrain: GrainWithTenant, IGameMasterGrain
{
    public Guid GameId { get; private init; }
    public Game GameFromDB { get; private set; }
    public Chess Game { get; private set; }
    
    public GameMasterGrain(ITenantFactory tenantFactory, Guid gameId) : 
        base(tenantFactory)
    {
        GameId = gameId;
    }
    
    // Override methods
    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        // Load FEN from DB if exists
        var tenant = GetTenant();

        GameFromDB = await tenant.Games.GetById(GameId, cancellationToken);
        Game = Builders.ChessBuild(GameFromDB.Fen);
        
        await base.OnActivateAsync(cancellationToken);
    }
    
    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        // Update FEN in DB
        var tenant = GetTenant();
        
        var currentFen = Game.GetFen();
        GameFromDB.UpdateFEN(currentFen);
        await tenant.Games.Update(GameFromDB, cancellationToken);
        await tenant.CommitAsync(cancellationToken);
        
        await base.OnDeactivateAsync(reason, cancellationToken);
    }
    
    public async Task<string> Move(Guid playerId, Guid gameId, string move)
    {
        await Game.SafeMove(move);
        return Game.GetFen();
    }
}