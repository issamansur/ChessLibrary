using Akka.Actor;
using Akka.Event;
using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Infrastructure.Actors.AkkaNet.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ChessMaster.Infrastructure.Actors.AkkaNet;

public class ChessMaster: MyUntypedActor
{
    private Dictionary<Guid, IActorRef> GameMasters { get; } = new();
    private Dictionary<IActorRef, Guid> Guids { get; } = new();
    
    public ChessMaster(IServiceScopeFactory serviceScopeFactory)
    : base(serviceScopeFactory)
    {
    }
    
    public static Props Props(IServiceScopeFactory scopeFactory) =>
        Akka.Actor.Props.Create(() => new ChessMaster(scopeFactory));

    private ILoggingAdapter Log { get; } = Context.GetLogger();
    
    protected override void PreStart() => Log.Info($"ChessMaster started");
    protected override void PostStop() => Log.Info($"ChessMaster stopped");
    
    protected override void OnReceive(object message)
    {
        Log.Info($"Received message by ChessMaster: {message}");
        
        switch (message)
        {
            case JoinGameCommand  joinGameMessage:
                Log.Info($"Forward message to GameMaster for game: {joinGameMessage.GameId}");
                GetGameMaster(joinGameMessage.GameId).Forward(joinGameMessage);
                break;
            
            case MoveGameCommand moveGameMessage:
                Log.Info($"Forward message to GameMaster for game: {moveGameMessage.GameId}");
                GetGameMaster(moveGameMessage.GameId).Forward(moveGameMessage);
                break;
            
            case Terminated terminated:
                var gameId = Guids[terminated.ActorRef];
                Log.Info($"GameMaster for game: {gameId} removed from GameMaster");
                
                GameMasters.Remove(gameId);
                Guids.Remove(terminated.ActorRef);
                
                break;
            
            default:
                Log.Warning($"Unhandled message by ChessMaster: {message}");
                
                Unhandled(message);
                break;
        }
    }
    
    private IActorRef GetGameMaster(Guid gameId)
    {
        // Create gameMaster if not exists
        if (!GameMasters.ContainsKey(gameId))
        {
            // Create gameMaster
            var gameMaster = Context.ActorOf(
                GameMaster.Props(_serviceScopeFactory, gameId), 
                $"game-{gameId}"
            );
            
            // Add to dictionaries
            GameMasters.Add(gameId, gameMaster);
            Guids.Add(gameMaster, gameId);
                        
            // Subscribe to gameMaster on Stop/Error
            Context.Watch(gameMaster);
        }
        
        return GameMasters[gameId];
    }
}