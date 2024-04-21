using ChessMaster.Application.Games.Filters;

namespace ChessMaster.Application.Games.Queries;

public class SearchGameQuery: IRequest<List<Game>>
{
    public GameFilter Filter { get; }
    
    public SearchGameQuery(GameFilter filter)
    {
        Filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }
}