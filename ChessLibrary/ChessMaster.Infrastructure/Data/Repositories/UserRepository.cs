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
        return Task.FromResult(_context.Users.Find(new object?[] { id }) 
                               ?? throw new ArgumentNullException(nameof(User)));
    }

    public Task<User?> TryGetByUsername(string userName, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Users.FirstOrDefault(x => x.Username == userName));
    }

    public Task<User> GetByUsername(string userName, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Users.FirstOrDefault(x => x.Username == userName)
                               ?? throw new ArgumentNullException(nameof(User)));
    }
}