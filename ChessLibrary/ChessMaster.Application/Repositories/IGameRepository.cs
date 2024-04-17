namespace ChessMaster.Application.Repositories;

public interface IGameRepository: ICRUDRepository<Game>
{
    Task<IReadOnlyCollection<Game>> GetByUser(Guid playerId, CancellationToken cancellationToken);
}