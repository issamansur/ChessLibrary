using ChessMaster.Application.Games.Filters;

namespace ChessMaster.Infrastructure.Data.Repositories;

public class GameRepository: IGameRepository
{
    private readonly ChessMasterDbContext _context;
    public GameRepository(ChessMasterDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task Create(Game entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task Update(Game entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Game> GetById(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<Game>> GetByUser(Guid playerId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<Game>> GetCreatedGames(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<Game>> Search(GameFilter filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}