using ChessMaster.Application.Games.Filters;

namespace ChessMaster.Application.Repositories;

public interface IGameRepository : ICRUDRepository<Game>
{
    Task<IReadOnlyCollection<Game>> GetByUser(Guid playerId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Game>> GetCreatedGames(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Game>> Search(GameFilter filter, CancellationToken cancellationToken);
}