namespace ChessMaster.Application.Repositories;

public interface ITenantRepository
{
    IUserRepository Users { get; }
    IAccountRepository Accounts { get; }
    IGameRepository Games { get; }
    
    Task CommitAsync(CancellationToken cancellationToken);
}