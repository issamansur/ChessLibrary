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
        throw new NotImplementedException();
    }

    public async Task Update(User entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetById(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> TryGetByUsername(string userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetByUsername(string userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}