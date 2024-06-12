using ChessMaster.Application.CQRS.Games.Filters;

namespace ChessMaster.Infrastructure.Data.Repositories;

public class GameRepository: IGameRepository
{
    private readonly ChessMasterDbContext _context;
    public GameRepository(ChessMasterDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public void Create(Game entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Games.Add(entity);
    }
    
    public void Update(Game entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        _context.Games.Update(entity);
    }
    
    public Game GetById(Guid id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var result = _context.Games
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        
        return result ?? throw new ArgumentNullException(nameof(Game));
    }
    
    public Game? TryGetById(Guid id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var result = _context.Games
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        
        return result;
    }

    public Task CreateAsync(Game entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Games.Add(entity);
        
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Game entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        _context.Games.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task<Game> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var result = _context.Games
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        
        return Task.FromResult(result ?? throw new ArgumentNullException(nameof(Game)));
    }
    
    public Task<Game?> TryGetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var result = _context.Games
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);
        
        return Task.FromResult(result);
    }

    public Task<IReadOnlyCollection<Game>> Search(GameFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var query = _context.Games.AsNoTracking();
    
        if (filter.PlayerId.HasValue)
        {
            query = query
                .Where(x => x.WhitePlayerId == filter.PlayerId || x.BlackPlayerId == filter.PlayerId);
        }

        if (filter.GameState.HasValue)
        {
            query = query
                .Where(x => x.GameState == filter.GameState);
        }

        query = query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize);

        var result = query.ToList() as IReadOnlyCollection<Game>;
        
        return Task.FromResult(result);
    }
}