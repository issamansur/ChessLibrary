namespace ChessMaster.Application.Repositories;

public interface IAccountRepository: ICRUDRepository<Account>
{
    Task<bool> Exists(string userName, CancellationToken cancellationToken);
    Task<Account?> GetByEmail(string userName, CancellationToken cancellationToken);
}