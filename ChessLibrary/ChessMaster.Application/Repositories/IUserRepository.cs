namespace ChessMaster.Application.Repositories;

public interface IUserRepository: ICrudRepository<User>
{
    Task<User?> TryGetByUsername(string username, CancellationToken cancellationToken);
    Task<User> GetByUsername(string username, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<User>> Search(string query, CancellationToken cancellationToken);
}