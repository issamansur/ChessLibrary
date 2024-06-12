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
                Log.Info($"Received JoinGameMessage for game: {joinGameMessage.GameId}");
                GetGameMaster(joinGameMessage.GameId).Forward(joinGameMessage);
                
                break;
            
            // Handle Move
            case MoveGameCommand moveGameMessage:
                Log.Info($"Received MoveGameMessage for game: {moveGameMessage.GameId}");
                GetGameMaster(moveGameMessage.GameId).Forward(moveGameMessage);
                
                break;
            
            // Handle Terminated message
            case Terminated terminated:
                var gameId = Guids[terminated.ActorRef];
                Log.Info($"GameMaster for game: {gameId} terminated");
                
                GameMasters.Remove(gameId);
                Guids.Remove(terminated.ActorRef);
                
                break;
                
            // Handle unexpected messages
            default:
                Log.Info($"Unhandled message by ChessMaster: {message}");
                
                Unhandled(message);
                break;
        }
    }
    
    // Handlers for main logic
    private void JoinCommandHandler(JoinGameCommand joinGameMessage)
    {
        if (GameMasters.ContainsKey(joinGameMessage.GameId))
        {
            throw new InvalidOperationException($"Game: {joinGameMessage.GameId} already exists");
        }
        
        // UpdateAsync game state in database
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var tenantFactory = scope.ServiceProvider.GetRequiredService<ITenantFactory>();
            var tenant = tenantFactory.GetRepository();
            
            var game = tenant.Games.TryGetById(joinGameMessage.GameId);

            if (game is null)
            {
                Context.Sender.Tell(new InvalidOperationException($"Game: {joinGameMessage.GameId} not found"));
                throw new InvalidOperationException($"Game: {joinGameMessage.GameId} not found");
            }

            game.Join(joinGameMessage.PlayerId);

            tenant.Games.Update(game);
            tenant.Commit();
        }

        // Added actor for game
        GetGameMaster(joinGameMessage.GameId);
        
        GameMasters[joinGameMessage.GameId].Forward(joinGameMessage);
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