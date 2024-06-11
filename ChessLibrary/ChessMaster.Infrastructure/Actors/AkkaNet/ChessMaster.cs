using Akka.Actor;
using Akka.Event;
using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Infrastructure.Actors.AkkaNet.Common;
using ChessMaster.Infrastructure.Actors.Common;
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
    
    protected override void PreStart()
    {
        Log.Info($"ChessMaster started");
    }
    
    protected override void PostStop()
    {
        Log.Info($"ChessMaster stopped");
    }
    
    protected override void OnReceive(object messageWithCt)
    {
        Log.Info($"Received message by ChessMaster: {messageWithCt}");
        var message = messageWithCt;
        var cancellationToken = ((ValueTuple<object, CancellationToken>)messageWithCt).Item2;
        Log.Info($"Received message by ChessMaster: {message}");
        
        switch (message)
        {
            case JoinGameCommand joinGameMessage:
                Log.Info($"Received JoinGameMessage for game: {joinGameMessage.GameId}");
                
                // Main logic
                if (!GameMasters.ContainsKey(joinGameMessage.GameId))
                {
                    // Update game state in database
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var tenantFactory = scope.ServiceProvider.GetRequiredService<ITenantFactory>();
                        var tenant = tenantFactory.GetRepository();
                        var game = tenant.Games.TryGet(joinGameMessage.GameId, cancellationToken).Result;

                        if (game is null)
                        {
                            throw new InvalidOperationException($"Game: {joinGameMessage.GameId} not found");
                        }

                        game.Join(joinGameMessage.PlayerId);

                        tenant.Games.Update(game, cancellationToken);
                        tenant.CommitAsync(cancellationToken);
                    }

                    // Added actor for game
                    var gameMaster = Context.ActorOf(
                        GameMaster.Props(
                            _serviceScopeFactory, 
                            joinGameMessage.GameId
                        ), 
                        $"game-{joinGameMessage.GameId}"
                    );
                    
                    // Added actor's info to dictionaries
                    GameMasters.Add(joinGameMessage.GameId, gameMaster);
                    Guids.Add(gameMaster, joinGameMessage.GameId);
                    
                    // Subscribe to gameMaster on Stop/Error
                    Context.Watch(gameMaster);
                    //GameMasters[joinGameMessage.GameId].Forward(joinGameMessage);
                    
                    Sender.Tell(joinGameMessage.GameId);
                }
                else
                {
                    throw new InvalidOperationException($"Game: {joinGameMessage.GameId} already exists");
                }
                
                break;
            
            case MoveGameCommand moveGameMessage:
                Log.Info($"Received MoveGameMessage for game: {moveGameMessage.GameId}");
                
                // Unexpected behavior (Only if server restarts)
                if (!GameMasters.ContainsKey(moveGameMessage.GameId))
                {
                    var gameMaster = Context.ActorOf(
                        GameMaster.Props(_serviceScopeFactory, moveGameMessage.GameId), 
                        $"game-{moveGameMessage.GameId}"
                    );
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