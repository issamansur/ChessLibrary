namespace ChessMaster.Application.Repositories;

public interface IGameResultRepository: IGettableRepository<GameResult>
{
    Task Create(GameResult gameResult, CancellationToken cancellationToken);
}