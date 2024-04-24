using ChessMaster.Application.Games.Filters;

namespace ChessMaster.Application.Repositories;

public interface IGameRepository : ICRUDRepository<Game>
{
    Task<IReadOnlyCollection<Game>> Search(GameFilter filter, CancellationToken cancellationToken);
}