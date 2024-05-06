namespace ChessMaster.Application.Repositories;

public interface IUserRepository: ICRUDRepository<User>
{
    Task<User?> TryGetByUsername(string userName, CancellationToken cancellationToken);
    Task<User> GetByUsername(string username, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<User>> Search(string query, CancellationToken cancellationToken);
}