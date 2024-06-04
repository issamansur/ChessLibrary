using Akka.Actor;

namespace ChessMaster.Infrastructure.Actors.AkkaNet;

public class AkkaNetSystem: IActorService
{
    readonly IActorRef _chessMaster;
    
    public AkkaNetSystem()
    {
        // Not working with using statement
        /*
        using (var system = ActorSystem.Create("chess-system"))
        {
            _chessMaster = system.ActorOf(ChessMaster.Props(), "chess-master");
        }
        */
        var system = ActorSystem.Create("chess-system");
        _chessMaster = system.ActorOf(ChessMaster.Props(), "chess-master");
    }
    
    public void Tell(object message)
    {
        Console.WriteLine($"Received message by AkkaNetSystem: {message}");
        _chessMaster.Tell(message);
    }

    public async Task<T> Ask<T>(object message)
    {
        Console.WriteLine($"Received message by AkkaNetSystem: {message}");
        return await _chessMaster.Ask<T>(message);
    }
}