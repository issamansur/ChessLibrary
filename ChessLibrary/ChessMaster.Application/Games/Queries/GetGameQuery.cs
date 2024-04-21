namespace ChessMaster.Application.Games.Queries;

public class GetGameQuery: IRequest<Game>
{
    public Guid GameId { get; }

    public GetGameQuery(Guid id)
    {
        GameId = id;
    }
}