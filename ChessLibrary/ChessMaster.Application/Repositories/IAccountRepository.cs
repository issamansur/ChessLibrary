namespace ChessMaster.Application.Repositories;

public interface IAccountRepository: ICrudRepository<Account>
{
    Task<Account?> TryGetByEmail(string email, CancellationToken cancellationToken);
    Task<Account> GetByEmail(string email, CancellationToken cancellationToken);
}