namespace ChessMaster.Application.Repositories;

public interface IGameRepository: ICRUDRepository<Game>
{
    Task<IReadOnlyCollection<Game>> GetByUser(Guid playerId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Game>> GetAll(
        int count = 100,
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}