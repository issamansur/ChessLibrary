using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Common;
using Orleans;

namespace ChessMaster.Infrastructure.Actors.MicrosoftOrleans;

public class MicrosoftOrleansSystem: IActorService
{
    IGrainFactory GrainFactory { get; init; }
    
    public MicrosoftOrleansSystem(IGrainFactory grainFactory)
    {
        GrainFactory = grainFactory;
    }
    
    public Task Tell<T>(T message, CancellationToken cancellationToken)
    {
        /*
        Console.WriteLine($"Received message by MicrosoftOrleansSystem: {message}");
        if (T is MoveGameCommand)
        {
            return chessMaster.Move(moveGameCommand.GameId, moveGameCommand.PlayerId, moveGameCommand.Move);
        }
        
        return chessMaster.Move(0, 0, message);
        */
        return Task.CompletedTask;
    }
    
    public Task<TResult> Ask<TRequest, TResult>(TRequest message, CancellationToken cancellationToken)
    {
        /*
        Console.WriteLine($"Received message by MicrosoftOrleansSystem: {message}");
        return chessMaster.GetState();
        */
        return Task.FromResult(default(TResult));
    }
}