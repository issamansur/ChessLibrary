namespace ChessMaster.Infrastructure.Data.Repositories;

public class UserRepository: IUserRepository
{
    private readonly ChessMasterDbContext _context;
    public UserRepository(ChessMasterDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public Task Create(User entity, CancellationToken cancellationToken)
    { 
        ArgumentNullException.ThrowIfNull(entity);
        
        _context.Users.Add(entity);
        
        return Task.CompletedTask;
    }

    public Task Update(User entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        _context.Users.Update(entity);

        return Task.CompletedTask;
    }

    public Task<User> GetById(Guid id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var result = _context.Users
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        
        return Task.FromResult(result ?? throw new ArgumentNullException(nameof(User)));
    }

    public Task<User?> TryGetByUsername(string username, CancellationToken cancellationToken)
    {
        var result = _context.Users
            .AsNoTracking()
            .FirstOrDefault(x => x.Username == username);
        
        return Task.FromResult(result);
    }

    public Task<User> GetByUsername(string username, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Users.AsNoTracking().FirstOrDefault(x => x.Username == username)
                               ?? throw new ArgumentNullException(nameof(User)));
    }

    public Task<IReadOnlyCollection<User>> Search(string query, CancellationToken cancellationToken)
    {
        var result = _context.Users
            .AsNoTracking()
            .Where(x => EF.Functions.Like(x.Username, $"%{query}%"))
            .ToList() as IReadOnlyCollection<User>;

        return Task.FromResult(result);
    }
}