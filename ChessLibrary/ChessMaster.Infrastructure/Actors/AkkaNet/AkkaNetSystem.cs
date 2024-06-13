using Akka.Actor;
using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Application.CQRS.Games.Queries;
using ChessMaster.Infrastructure.Actors.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChessMaster.Infrastructure.Actors.AkkaNet;

public class AkkaNetSystem: IChessActorService, IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private IActorRef _chessMaster;
    
    public AkkaNetSystem(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    // Handlers
    public async Task<Game> GetGameAsync(GetGameQuery getGameQuery, CancellationToken cancellationToken)
    {
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine($"Received message by AkkaNetSystem: {getGameQuery}");
        
        return await _chessMaster.Ask<Game>(
            message: getGameQuery, 
            timeout: TimeSpan.FromSeconds(3),
            cancellationToken: cancellationToken
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

    // IHostedService
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var system = ActorSystem.Create("chess-system");
        _chessMaster = system.ActorOf(
            ChessMaster.Props(_scopeFactory), 
            "chess-master"
        );
        
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _chessMaster.GracefulStop(TimeSpan.FromSeconds(3));
    }
}