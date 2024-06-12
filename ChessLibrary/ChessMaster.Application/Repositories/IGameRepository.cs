using ChessMaster.Application.CQRS.Games.Filters;

namespace ChessMaster.Application.Repositories;

public interface IGameRepository : ICrudRepository<Game>
{
    void Create(Game entity);
    void Update(Game entity);
    Game GetById(Guid id);
    Game? TryGetById(Guid id);
    Task<Game?> TryGetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Game>> Search(GameFilter filter, CancellationToken cancellationToken);
}