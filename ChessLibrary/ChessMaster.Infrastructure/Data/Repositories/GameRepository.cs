using ChessMaster.Application.Games.Filters;

namespace ChessMaster.Infrastructure.Data.Repositories;

public class GameRepository: IGameRepository
{
    private readonly ChessMasterDbContext _context;
    public GameRepository(ChessMasterDbContext dbContext)
    {
        _context = dbContext;
    }

    public Task Create(Game entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Games.Add(entity);
        
        return Task.CompletedTask;
    }

    public Task Update(Game entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        _context.Games.Update(entity);
        
        return Task.CompletedTask;
    }

    public Task<Game> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_context.Games.Find(new object?[] { id }) 
                               ?? throw new ArgumentNullException(nameof(Game)));
    }

    public Task<IReadOnlyCollection<Game>> Search(GameFilter filter, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        IQueryable<Game> query = _context.Games;
    
        if (filter.PlayerId.HasValue)
        {
            query = query
                .Where(x => x.WhitePlayerId == filter.PlayerId || x.BlackPlayerId == filter.PlayerId);
        }

        if (filter.GameState.HasValue)
        {
            query = query
                .Where(x => x.State == filter.GameState);
        }

        return Task.FromResult(query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList() as IReadOnlyCollection<Game>);
    }
}