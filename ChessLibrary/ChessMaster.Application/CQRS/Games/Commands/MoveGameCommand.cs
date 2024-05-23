namespace ChessMaster.Application.CQRS.Games.Commands;

public class MoveGameCommand: IRequest<Game>
{
    public Guid GameId { get;  }
    public Guid PlayerId { get; }
    public string Move { get; }
    
    public MoveGameCommand(Guid gameId, Guid playerId, string move)
    {
        GameId = gameId;
        PlayerId = playerId;
        Move = move;
    }
}