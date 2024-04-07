namespace ChessMaster.Application.Repositories;

public interface IAccountRepository: IGettableRepository<Account>
{
    Task Register(Account account, CancellationToken cancellationToken);
    Task Login(Account account, CancellationToken cancellationToken);
}