namespace ChessMaster.Application.Repositories;

public interface IAccountRepository: ICRUDRepository<Account>
{
    Task<Account?> TryGetByEmail(string email, CancellationToken cancellationToken);
    Task<Account> GetByEmail(string email, CancellationToken cancellationToken);
}