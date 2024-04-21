using ChessMaster.Domain.States;

namespace ChessMaster.Application.Games.Filters;

public class GameFilter
{
    public Guid? PlayerId { get; }
    public State? PlayerState { get; }
    
    public int PageNumber { get; }
    public int PageSize { get; }
    
    public GameFilter(Guid? playerId = null, State? playerState = null, int pageNumber = 1, int pageSize = 100)
    {
        PlayerId = playerId;
        PlayerState = playerState;
        PageNumber = pageNumber;
        PageSize = pageSize;
        
        var validator = new GameFilterValidator();
        validator.ValidateAndThrow(this);
    }
}