using ChessMaster.Application.CQRS.Games.Filters;

namespace ChessMaster.Application.CQRS.Games.Queries;

public class SearchGameQuery: IRequest<IReadOnlyCollection<Game>>
{
    public GameFilter Filter { get; }
    
    public SearchGameQuery(GameFilter filter)
    {
        Filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }
}