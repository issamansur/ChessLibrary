namespace ChessMaster.Application.Repositories;

public interface IUserRepository: ICRUDRepository<User>
{
    Task<bool> Exists(string userName, CancellationToken cancellationToken);
    Task<User?> GetByUserName(string userName, CancellationToken cancellationToken);
}