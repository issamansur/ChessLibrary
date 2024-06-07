using Orleans;

namespace ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Common;

public interface IGameMasterGrain: IGrainWithIntegerKey
{
    // Task CreateGameAsync(Guid gameId, Guid playerId);
    //Task JoinGameAsync(Guid gameId, Guid playerId);
    Task<string> Move(Guid gameId, Guid playerId, string move);
}