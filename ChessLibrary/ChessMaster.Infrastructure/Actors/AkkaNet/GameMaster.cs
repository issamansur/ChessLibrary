using Akka.Actor;
using Akka.Event;
using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.ChessModels;
using ChessMaster.ChessModels.States;
using ChessMaster.ChessModels.Utils;
using ChessMaster.Domain.States;
using ChessMaster.Infrastructure.Actors.AkkaNet.Common;
using ChessMaster.Infrastructure.Actors.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ChessMaster.Infrastructure.Actors.AkkaNet;

public class GameMaster: MyUntypedActor
{
    public Guid GameId { get; init; }
    public Game? Game { get; private set; }
    public Chess Chess { get; private set; }
    public State CurrentState => Game?.GameState ?? State.None;
    
    public GameMaster(
        IServiceScopeFactory serviceScopeFactory,
        Guid gameId) 
        : base(serviceScopeFactory)
    {
        GameId = gameId;
        Game = null;
        Chess = new Chess();
    }
    
    public static Props Props(IServiceScopeFactory serviceScopeFactory, Guid gameId) =>
        Akka.Actor.Props.Create(() => new GameMaster(serviceScopeFactory, gameId));

    private ILoggingAdapter Log { get; } = Context.GetLogger();
    
    protected override void PreStart()
    {
        Log.Info($"GameMaster for game: {GameId} started");
        LoadGameState();
    }
    
    protected override void PostStop()
    {
        Log.Info($"GameMaster for game: {GameId} stopped");
        SaveGameState();
    }
    
    protected override void OnReceive(object message)
    {
        Log.Info($"Received message by GameMaster for game: {GameId}: {message}");
        
        switch (message)
        {
            case JoinGameCommand joinGameCommand:
                Log.Info($"Received JoinGameMessage for game: {GameId}");
                
                Game!.Join(joinGameCommand.PlayerId);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var tenantFactory = scope.ServiceProvider.GetRequiredService<ITenantFactory>();
                    var tenant = tenantFactory.GetRepository();
                    
                    tenant.Games.Update(Game);
                    tenant.Commit();
                }
                
                break;
            
            case MoveGameCommand moveGameCommand:
                Log.Info($"Received MoveGameMessage for game: {GameId}");
                
                Chess.Move(moveGameCommand.Move);
                Game!.UpdateFEN(Chess.GetFen());
                Sender.Tell(Game);
                
                if (Game!.GameState == State.Finished)
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
    
    /// <summary>
    /// Load game state from database
    /// </summary>
    private void LoadGameState()
    {
        Game? game;
        
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var tenantFactory = scope.ServiceProvider.GetRequiredService<ITenantFactory>();
            var tenant = tenantFactory.GetRepository();
            
            game = tenant.Games.TryGetById(GameId);
        }
        
        if (game is null)
        {
            Log.Error($"Game: {GameId} not found in database");
            Context.Stop(Self);
            return;
        }
            
        Game = game;
        Chess = Builders.ChessBuild(game.Fen);
            
        Log.Info($"Game: {GameId} state loaded from database");
    }
    
    /// <summary>
    /// Save game state to database
    /// </summary>
    private void SaveGameState()
    {
        if (Game is null)
        {
            return;
        }
        
        using var scope = _serviceScopeFactory.CreateScope();
        
        var tenantFactory = scope.ServiceProvider.GetRequiredService<ITenantFactory>();
        var tenant = tenantFactory.GetRepository();
        
        Game.UpdateFEN(Chess.GetFen());
        tenant.Games.Update(Game);
        tenant.Commit();
            
        Log.Info($"Game: {GameId} state saved to database");
    }
}