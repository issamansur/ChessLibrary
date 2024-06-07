using Akka.Actor;
using Akka.Event;
using ChessMaster.ChessModels;
using ChessMaster.ChessModels.States;
using ChessMaster.ChessModels.Utils;
using ChessMaster.Infrastructure.Actors.AkkaNet.Common;
using ChessMaster.Infrastructure.Actors.Common;
using ChessMaster.Infrastructure.Data.Common;

namespace ChessMaster.Infrastructure.Actors.AkkaNet;

public class GameMaster: UntypedActor //ActorWithTenant
{
    public Guid GameId { get; init; }
    public Chess Game { get; init; }
    
    public GameMaster(
        /*TenantFactory tenantFactory,*/
        Guid gameId, string fen = ChessExt.DefaultFen) 
        : base(/*tenantFactory*/)
    {
        GameId = gameId;
        Game = Builders.ChessBuild(fen);
    }
    
    public static Props Props(Guid gameId, string fen = ChessExt.DefaultFen) =>
        Akka.Actor.Props.Create(() => new GameMaster(gameId, fen));

    private ILoggingAdapter Log { get; } = Context.GetLogger();
    
    protected override void PreStart()
    {
        Log.Info($"ChessMaster for game: {GameId} started");
    }
    
    protected override void PostStop()
    {
        Log.Info($"ChessMaster for game: {GameId} stopped");
    }
    
    protected override void OnReceive(object message)
    {
        Log.Info($"Received message by ChessMaster for game: {GameId}: {message}");
        
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
                Move(moveGameMessage.Move);
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
    
    private void Move(string move)
    {
        try
        {
            Game.Move(move);
        }
        catch (Exception e)
        {
            Log.Error(e, $"Error while moving in game: {GameId}");
        }
    }
}