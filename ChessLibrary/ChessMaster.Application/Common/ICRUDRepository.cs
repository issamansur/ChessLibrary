namespace ChessMaster.Application.Common;

public interface ICRUDRepository<T>
{
    Task Create(T entity, CancellationToken cancellationToken);
    Task Update(T entity, CancellationToken cancellationToken);
    Task Delete(T entity, CancellationToken cancellationToken);
    
    Task<T?> GetById(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<T>> GetAll(
        int count = 100,
        int offset = 0,
        CancellationToken cancellationToken = default
        );
}