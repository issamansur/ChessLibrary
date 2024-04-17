namespace ChessMaster.Application.Games;

public class JoinGameCommand: IRequest<Game>
{
    public Guid GameId { get; }
    public Guid PlayerId { get; }
    
    public JoinGameCommand(Guid gameId, Guid playerId)
    {
        GameId = gameId;
        PlayerId = playerId;
    }
}