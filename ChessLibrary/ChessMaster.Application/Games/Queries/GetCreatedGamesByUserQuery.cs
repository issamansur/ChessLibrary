namespace ChessMaster.Application.Games.Queries;

public class GetCreatedGamesByUserQuery: IRequest<IReadOnlyCollection<Game>>
{
    public Guid UserId { get; set; }

    public GetCreatedGamesByUserQuery(Guid userId)
    {
        UserId = userId;
    }
}