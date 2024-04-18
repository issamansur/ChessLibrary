namespace ChessMaster.Application.Common;

public interface ICRUDRepository<T>
{
    Task Create(T entity, CancellationToken cancellationToken);
    Task Update(T entity, CancellationToken cancellationToken);
    
    Task<T> GetById(Guid id, CancellationToken cancellationToken);
}