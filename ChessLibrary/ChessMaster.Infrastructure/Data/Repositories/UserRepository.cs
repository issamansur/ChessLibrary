namespace ChessMaster.Infrastructure.Data.Repositories;

public class UserRepository: IUserRepository
{
    private readonly ChessMasterDbContext _context;
    public UserRepository(ChessMasterDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task Create(User entity, CancellationToken cancellationToken)
    { 
        ArgumentNullException.ThrowIfNull(entity);

        await _context.Users.AddAsync(entity, cancellationToken);
    }

    public Task Update(User entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        _context.Users.Update(entity);

        return Task.CompletedTask;
    }

    public async Task<User> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users.FindAsync(new object?[] { id }, cancellationToken: cancellationToken) ?? throw new ArgumentNullException();
    }

    public async Task<User?> TryGetByUsername(string userName, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username == userName, cancellationToken);
    }

    public async Task<User> GetByUsername(string userName, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username == userName, cancellationToken) ?? throw new ArgumentNullException();
    }
}