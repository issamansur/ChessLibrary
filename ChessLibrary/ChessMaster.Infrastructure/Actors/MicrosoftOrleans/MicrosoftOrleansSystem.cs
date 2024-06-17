using ChessMaster.Application.CQRS.Games.Commands;
using ChessMaster.Application.CQRS.Games.Queries;
using ChessMaster.Infrastructure.Actors.Common;
using ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Common;
using Microsoft.Extensions.Hosting;
using Orleans;

namespace ChessMaster.Infrastructure.Actors.MicrosoftOrleans;

public class MicrosoftOrleansSystem: IChessActorService 
{
    IGrainFactory GrainFactory { get; init; }
    
    public MicrosoftOrleansSystem(IGrainFactory grainFactory)
    {
        GrainFactory = grainFactory;
    }
    
    public async Task<Game> GetGameAsync(GetGameQuery getGameQuery, CancellationToken cancellationToken)
    {
        IGameMasterGrain gameMasterMaster = GrainFactory.GetGrain<IGameMasterGrain>(getGameQuery.GameId);
        return await gameMasterMaster.GetGameAsync(cancellationToken);
    }

    public async Task<Game> JoinGameAsync(JoinGameCommand joinGameCommand, CancellationToken cancellationToken)
    {
        IGameMasterGrain gameMasterMaster = GrainFactory.GetGrain<IGameMasterGrain>(joinGameCommand.GameId);
        return await gameMasterMaster.JoinGameAsync(joinGameCommand.PlayerId, cancellationToken);
    }

    public async Task<Game> MoveGameAsync(MoveGameCommand moveGameCommand, CancellationToken cancellationToken)
    {
        IGameMasterGrain gameMasterMaster = GrainFactory.GetGrain<IGameMasterGrain>(moveGameCommand.GameId);
        return await gameMasterMaster.GetGameAsync(cancellationToken);
    }
}