namespace ChessMaster.Infrastructure.Data.Repositories;

public class AccountRepository: IAccountRepository
{
    private readonly ChessMasterDbContext _context;
    public AccountRepository(ChessMasterDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task Create(Account entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task Update(Account entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Account> GetById(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Account?> TryGetByEmail(string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Account> GetByEmail(string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}