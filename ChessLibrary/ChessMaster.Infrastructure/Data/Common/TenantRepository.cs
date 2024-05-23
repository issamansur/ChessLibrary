using ChessMaster.Infrastructure.Data.Repositories;

namespace ChessMaster.Infrastructure.Data.Common;

public class TenantRepository: ITenantRepository
{
    private readonly ChessMasterDbContext _context;
    public IUserRepository Users { get; }
    public IAccountRepository Accounts { get; }
    public IGameRepository Games { get; }
    
    public TenantRepository(ChessMasterDbContext dbContext)
    {
        _context = dbContext;
        
        Users = new UserRepository(dbContext);
        Accounts = new AccountRepository(dbContext);
        Games = new GameRepository(dbContext);
    }
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}