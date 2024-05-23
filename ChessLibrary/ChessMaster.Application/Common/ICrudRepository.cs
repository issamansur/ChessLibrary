namespace ChessMaster.Application.Common;

public interface ICrudRepository<T>
{
    Task Create(T entity, CancellationToken cancellationToken);
    Task Update(T entity, CancellationToken cancellationToken);
    
    Task<T> GetById(Guid id, CancellationToken cancellationToken);
}