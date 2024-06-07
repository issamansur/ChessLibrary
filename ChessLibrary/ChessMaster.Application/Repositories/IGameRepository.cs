using ChessMaster.Application.CQRS.Games.Filters;

namespace ChessMaster.Application.Repositories;

public interface IGameRepository : ICrudRepository<Game>
{
    Task<Game?> TryGet(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Game>> Search(GameFilter filter, CancellationToken cancellationToken);
}