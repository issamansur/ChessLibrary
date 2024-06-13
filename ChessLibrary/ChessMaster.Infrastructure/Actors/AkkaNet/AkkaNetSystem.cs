using Akka.Actor;
using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Infrastructure.Actors.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ChessMaster.Infrastructure.Actors.AkkaNet;

public class AkkaNetSystem: IChessActorService
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

    public async Task<Game> JoinGameAsync(JoinGameCommand joinGameCommand, CancellationToken cancellationToken)
    {
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine($"Received message by AkkaNetSystem: {joinGameCommand}");
        
        return await _chessMaster.Ask<Game>(
            message: joinGameCommand,
            timeout: TimeSpan.FromSeconds(3),
            cancellationToken: cancellationToken
        );
    }

    public async Task<Game> MoveGameAsync(MoveGameCommand moveGameCommand, CancellationToken cancellationToken)
    {
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine($"Received message by AkkaNetSystem: {moveGameCommand}");
        
        return await _chessMaster.Ask<Game>(
            message: moveGameCommand, 
            timeout: TimeSpan.FromSeconds(3),
            cancellationToken: cancellationToken
        );
    }
}