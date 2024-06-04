using Akka.Actor;
using Akka.Event;
using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Infrastructure.Actors.ActorsMessages;

namespace ChessMaster.Infrastructure.Actors.AkkaNet;

public class ChessMaster: UntypedActor
{
    private Dictionary<Guid, IActorRef> GameMasters { get; } = new();
    private Dictionary<IActorRef, Guid> Guids { get; } = new();
    
    public ChessMaster()
    {
    }
    
    public static Props Props() =>
        Akka.Actor.Props.Create(() => new ChessMaster());

    private ILoggingAdapter Log { get; } = Context.GetLogger();
    
    protected override void PreStart()
    {
        Log.Info($"ChessMaster started");
    }
    
    protected override void PostStop()
    {
        Log.Info($"ChessMaster stopped");
    }

    protected override void OnReceive(object message)
    {
        Log.Info($"Received message by ChessMaster: {message}");
        
        switch (message)
        {
            case JoinGameCommand joinGameMessage:
                Log.Info($"Received JoinGameMessage for game: {joinGameMessage.GameId}");
                
                // Main logic
                if (!GameMasters.ContainsKey(joinGameMessage.GameId))
                {
                    var gameMaster = Context.ActorOf(GameMaster.Props(joinGameMessage.GameId), $"game-{joinGameMessage.GameId}");
                    GameMasters.Add(joinGameMessage.GameId, gameMaster);
                    Guids.Add(gameMaster, joinGameMessage.GameId);
                    
                    // Subscribe to gameMaster on Stop/Error
                    Context.Watch(gameMaster);
                    //GameMasters[joinGameMessage.GameId].Forward(joinGameMessage);
                }
                // Unexpected behavior
                else
                {
                    
                }
                
                break;
            
            case MoveGameCommand moveGameMessage:
                Log.Info($"Received MoveGameMessage for game: {moveGameMessage.GameId}");
                
                // Unexpected behavior (Only if server restarts)
                if (!GameMasters.ContainsKey(moveGameMessage.GameId))
                {
                    var gameMaster = Context.ActorOf(GameMaster.Props(moveGameMessage.GameId), $"game-{moveGameMessage.GameId}");
                    GameMasters.Add(moveGameMessage.GameId, gameMaster);
                    Guids.Add(gameMaster, moveGameMessage.GameId);
                    
                    // Subscribe to gameMaster on Stop/Error
                    Context.Watch(gameMaster);
                }
                
                // Main logic
                GameMasters[moveGameMessage.GameId].Forward(new MoveGameMessage(moveGameMessage.GameId, moveGameMessage.Move));
                
                break;
            
            // Handle Terminated message
            case Terminated terminated:
                Log.Info($"GameMaster for game: {terminated.ActorRef.Path.Name} terminated");
                
                var gameId = Guids[terminated.ActorRef];
                GameMasters.Remove(gameId);
                Guids.Remove(terminated.ActorRef);
                
                break;
                
            
            default:
                Unhandled(message);
                break;
        }
    }
}