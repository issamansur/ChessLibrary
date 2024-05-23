using ChessMaster.Application.CQRS.Games.Filters;

namespace ChessMaster.Application.Repositories;

public interface IGameRepository : ICrudRepository<Game>
{
    Task<IReadOnlyCollection<Game>> Search(GameFilter filter, CancellationToken cancellationToken);
}