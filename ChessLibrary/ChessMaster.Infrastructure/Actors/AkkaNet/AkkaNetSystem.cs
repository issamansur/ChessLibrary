using Akka.Actor;
using Microsoft.Extensions.DependencyInjection;

namespace ChessMaster.Infrastructure.Actors.AkkaNet;

public class AkkaNetSystem: IActorService
{
    private readonly IActorRef _chessMaster;
    
    public AkkaNetSystem(IServiceScopeFactory scopeFactory)
    {
        var system = ActorSystem.Create("chess-system");
        _chessMaster = system.ActorOf(
            ChessMaster.Props(scopeFactory), 
            "chess-master"
        );
    }
    
    
    
    public Task Tell<T>(T message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Received message by AkkaNetSystem: {message}");
        _chessMaster.Tell((message, cancellationToken));
        
        return Task.CompletedTask;
    }

    public async Task<TResult> Ask<TRequest, TResult>(TRequest message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Received message by AkkaNetSystem: {message}");
        return await _chessMaster.Ask<TResult>(message, cancellationToken);
    }
}