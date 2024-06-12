namespace ChessMaster.Application.Common;

public interface ITenantRepository
{
    IUserRepository Users { get; }
    IAccountRepository Accounts { get; }
    IGameRepository Games { get; }
    
    void Commit();
    Task CommitAsync(CancellationToken cancellationToken);
}