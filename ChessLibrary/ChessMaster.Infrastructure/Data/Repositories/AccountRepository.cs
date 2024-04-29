namespace ChessMaster.Infrastructure.Data.Repositories;

public class AccountRepository: IAccountRepository
{
    private readonly ChessMasterDbContext _context;
    public AccountRepository(ChessMasterDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public Task Create(Account entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Accounts.Add(entity);
        
        return Task.CompletedTask;
    }

    public Task Update(Account entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        _context.Accounts.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task<Account> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Accounts.Find(new object?[] { id })
                               ?? throw new ArgumentNullException(nameof(Account)));
    }

    public Task<Account?> TryGetByEmail(string email, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Accounts.FirstOrDefault(x => x.Email == email));
    }

    public Task<Account> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Accounts.FirstOrDefault(x => x.Email == email.ToLower())
                               ?? throw new ArgumentNullException(nameof(Account)));
    }
}