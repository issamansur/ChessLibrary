namespace ChessMaster.Application.Common;

public interface IGettableRepository<T>
{
    Task<T?> GetById(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<T>> GetAll(CancellationToken cancellationToken);
}