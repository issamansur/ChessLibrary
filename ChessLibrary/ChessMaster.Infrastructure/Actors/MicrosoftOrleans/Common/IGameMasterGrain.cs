using Orleans;

namespace ChessMaster.Infrastructure.Actors.MicrosoftOrleans.Common;

public interface IGameMasterGrain: IGrainWithGuidKey
{
    [ResponseTimeout("00:00:03")]
    Task<Game> GetGameAsync(CancellationToken cancellationToken);
    [ResponseTimeout("00:00:03")]
    Task<Game> JoinGameAsync(Guid playerId, CancellationToken cancellationToken);
    [ResponseTimeout("00:00:03")]
    Task<Game> MoveGameAsync(Guid playerId, string move, CancellationToken cancellationToken);
}