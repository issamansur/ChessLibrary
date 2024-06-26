namespace ChessMaster.Infrastructure.Data.Repositories;

public class AccountRepository: IAccountRepository
{
    private readonly ChessMasterDbContext _context;
    public AccountRepository(ChessMasterDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public Task CreateAsync(Account entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Accounts.Add(entity);
        
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Account entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        _context.Accounts.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task<Account> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var result = _context.Accounts
            .AsNoTracking()
            .FirstOrDefault(x => x.UserId == id);
        
        return Task.FromResult(result ?? throw new ArgumentNullException(nameof(Account)));
    }

    public Task<Account?> TryGetByEmail(string email, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(email);
        
        var result = _context.Accounts
            .AsNoTracking()
            // NO USE OF StringComparison.CurrentCultureIgnoreCase (EF does not support it)
            .FirstOrDefault(x => x.Email == email.ToLower());
        
        return Task.FromResult(result);
    }

    public Task<Account> GetByEmail(string email, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(email);
        
        var result = _context.Accounts
            .AsNoTracking()
            // NO USE OF StringComparison.CurrentCultureIgnoreCase (EF does not support it)
            .FirstOrDefault(x => x.Email == email.ToLower());
        
        return Task.FromResult(result ?? throw new ArgumentNullException(nameof(Account)));
    }
}