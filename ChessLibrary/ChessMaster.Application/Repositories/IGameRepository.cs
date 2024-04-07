namespace ChessMaster.Application.Repositories;

public interface IGameRepository: IGettableRepository<Game>
{
    Task Create(Game game, CancellationToken cancellationToken);
    Task Update(Game game, CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<Game>> GetByUser(Guid playerId, CancellationToken cancellationToken);
}