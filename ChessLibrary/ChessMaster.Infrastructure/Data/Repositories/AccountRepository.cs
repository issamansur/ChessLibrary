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
        ArgumentNullException.ThrowIfNull(entity);

        await _context.Accounts.AddAsync(entity, cancellationToken);
    }

    public Task Update(Account entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        _context.Accounts.Update(entity);
        
        return Task.CompletedTask;
    }

    public async Task<Account> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Accounts.FindAsync(new object?[] { id }, cancellationToken: cancellationToken) ?? throw new ArgumentNullException();
    }

    public async Task<Account?> TryGetByEmail(string email, CancellationToken cancellationToken)
    {
        return await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<Account> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email, cancellationToken) ?? throw new ArgumentNullException();
    }
}