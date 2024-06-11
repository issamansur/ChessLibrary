using Akka.Actor;
using Akka.Event;
using ChessMaster.ChessModels;
using ChessMaster.ChessModels.States;
using ChessMaster.ChessModels.Utils;
using ChessMaster.Infrastructure.Actors.AkkaNet.Common;
using ChessMaster.Infrastructure.Actors.Common;
using ChessMaster.Infrastructure.Data.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ChessMaster.Infrastructure.Actors.AkkaNet;

public class GameMaster: MyUntypedActor
{
    public Guid GameId { get; init; }
    public Chess Game { get; private set; }
    
    public GameMaster(
        IServiceScopeFactory serviceScopeFactory,
        Guid gameId) 
        : base(serviceScopeFactory)
    {
        GameId = gameId;
        Game = Builders.ChessBuild();
    }
    
    public static Props Props(IServiceScopeFactory serviceScopeFactory, Guid gameId) =>
        Akka.Actor.Props.Create(() => new GameMaster(serviceScopeFactory, gameId));

    private ILoggingAdapter Log { get; } = Context.GetLogger();
    
    protected override async void PreStart()
    {
        Log.Info($"GameMaster for game: {GameId} started");
        
        var cancellationToken = new CancellationToken();

        // is needed to using {} or just var scope = ServiceScopeFactory.CreateScope()
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var tenantFactory = scope.ServiceProvider.GetRequiredService<ITenantFactory>();
            var tenant = tenantFactory.GetRepository();
            
            var game = await tenant.Games.TryGet(GameId, cancellationToken);
            if (game is null)
            {
                Log.Info($"Game: {GameId} not found in database");
                return;
            }
            
            Game = Builders.ChessBuild(game.Fen);
            
            Log.Info($"Game: {GameId} state loaded from database");
        }
    }
    
    protected override async void PostStop()
    {
        Log.Info($"ChessMaster for game: {GameId} stopped");
        await SaveGameState();
    }
    
    protected override void OnReceive(object messageWithCt)
    {
        var (message, cancellationToken) = messageWithCt as Tuple<object, CancellationToken> ?? throw new InvalidOperationException();
        Log.Info($"Received message by GameMaster for game: {GameId}: {message}");
        
        switch (message)
        {
            case MoveGameMessage moveGameMessage:
                Log.Info($"Received MoveGameMessage for game: {GameId}");
                
                // Unexpected behavior
                if (moveGameMessage.GameId != GameId)
                {
                    Log.Error($"Received MoveGameMessage for wrong game: {moveGameMessage.GameId}");
                    break;
                }
                
                // Main logic
                Game.SafeMove(moveGameMessage.Move);
                Sender.Tell(Builders.ToFen(Game));
                
                if (Game.GameState is GameState.Checkmate or GameState.Stalemate)
                {
                    Log.Info($"Game: {GameId} is finished");
                    Context.Stop(Self);
                }
                
                break;
            
            default:
                Unhandled(message);
                break;
        }
    }
    
    private async Task SaveGameState()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        
        // Save game state to database
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var tenantFactory = scope.ServiceProvider.GetRequiredService<ITenantFactory>();
            var tenant = tenantFactory.GetRepository();
            
            var game = await tenant.Games.GetById(GameId, cancellationToken);
            game.UpdateFEN(Game.GetFen());
            await tenant.Games.Update(game, cancellationToken);
            
            Log.Info($"Game: {GameId} state saved to database");
        }
    }
}