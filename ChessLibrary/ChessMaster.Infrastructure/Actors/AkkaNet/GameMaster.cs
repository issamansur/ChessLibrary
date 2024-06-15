using Akka.Actor;
using Akka.Event;
using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Application.CQRS.Games.Queries;
using ChessMaster.ChessLibrary;
using ChessMaster.ChessLibrary.States;
using ChessMaster.Domain.States;
using ChessMaster.Infrastructure.Actors.AkkaNet.Common;
using ChessMaster.Infrastructure.Actors.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ChessMaster.Infrastructure.Actors.AkkaNet;

public class GameMaster: MyUntypedActor
{
    public Guid GameId { get; private init; }
    public Game? Game { get; private set; }
    public Chess Chess { get; private set; }
    
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

    protected override void PostRestart(Exception reason)
    {        
        Log.Info($"GameMaster for game: {GameId} restarted");
        
        base.PostRestart(reason);
    }

    protected override void OnReceive(object message)
    {
        Log.Info($"Received message by GameMaster for game: {GameId}: {message}");
        
        // Check if game exists
        if (Game is null)
        {
            SendError($"Game: {GameId} not found", true);
            return;
        }
        
        // Handle messages
        switch (message)
        {
            case GetGameQuery getGameQuery:
                Log.Info($"User get game: {GameId}");
                Sender.Tell(Game);
                
                break;
            
            case JoinGameCommand joinGameCommand:
                Log.Info($"Player: {joinGameCommand.PlayerId} try join the game: {GameId}");
                Join(joinGameCommand.PlayerId);
                break;
            
            case MoveGameCommand moveGameCommand:
                Log.Info($"Player: {moveGameCommand.PlayerId} try move: {moveGameCommand.Move} in game: {GameId}");
                Move(moveGameCommand.PlayerId, moveGameCommand.Move);
                break;
            
            default:
                Unhandled(message);
                break;
        }
    }
    
    // Handlers for messages
    private void Join(Guid playerId)
    {
        try
        {
            Game!.Join(playerId);
            
            Sender.Tell(Game);
            Log.Info($"Player: {playerId} joined game: {GameId}");
        }
        catch (InvalidOperationException e)
        {
            SendError(e.Message);
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var tenantFactory = scope.ServiceProvider.GetRequiredService<ITenantFactory>();
        var tenant = tenantFactory.GetRepository();
            
        tenant.Games.Update(Game!);
        tenant.Commit();
    }
    
    private void Move(Guid playerId, string move)
    {
        if (Game!.GameState == State.Created)
        {
            SendError($"Game: {GameId} is not started yet", true);
            return;
        }
        
        if (playerId != Game!.WhitePlayerId && playerId != Game!.BlackPlayerId)
        {
            SendError($"Player: {playerId} is not in game: {GameId}");
            return;
        }
                
        if (playerId == Game!.WhitePlayerId && Chess.ActiveColor == Color.Black ||
            playerId == Game!.BlackPlayerId && Chess.ActiveColor == Color.White)
        {
            SendError($"Player: {playerId} is not on turn in game: {GameId}");
            return;
        }
                
        if (char.IsUpper(move[0]) && Chess.ActiveColor == Color.Black ||
            char.IsLower(move[0]) && Chess.ActiveColor == Color.White)
        {
            SendError($"Invalid move: {move}");
            return;
        }
        
        // Move
        try
        {
            Chess.Move(move);
        } catch (Exception e)
        {
            SendError($"Error while moving in game: {GameId}");
            return;
        }
        
        Log.Info($"Successfully moved in game: {GameId}");
        Game!.UpdateFEN(Chess.GetFen());
        Sender.Tell(Game);
        
        // Check if game is finished
        if (Game!.GameState == State.Finished)
        {
            Log.Info($"Game: {GameId} is finished");
            Context.Stop(Self);
        }
    }
    
    // Handlers for events
    
    /// <summary>
    /// Load game state from database (returns false if game not found)
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
            Log.Info($"Game: {GameId} not found in database");
            return;
        }
            
        Game = game;
        Chess = game.Fen.ToChess();
            
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
        
        tenant.Games.Update(Game);
        tenant.Commit();
            
        Log.Info($"Game: {GameId} state saved to database");
    }
    
    // Extra methods
    private void SendError(string message, bool stop = false)
    {
        Log.Error(message);
        Sender.Tell(new Status.Failure(new InvalidOperationException(message)), Self);
        if (stop)
        {
            //Context.Stop(Self);
            Self.GracefulStop(TimeSpan.FromSeconds(1));
        }
    }
}