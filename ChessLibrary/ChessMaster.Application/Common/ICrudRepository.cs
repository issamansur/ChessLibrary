namespace ChessMaster.Application.Common;

public interface ICrudRepository<T>
{
    Task CreateAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}