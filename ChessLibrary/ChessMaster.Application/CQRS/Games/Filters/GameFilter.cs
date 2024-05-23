using ChessMaster.Domain.States;

namespace ChessMaster.Application.CQRS.Games.Filters;

public class GameFilter
{
    public Guid? PlayerId { get; }
    public State? GameState { get; }
    
    public int PageNumber { get; }
    public int PageSize { get; }
    
    public GameFilter(Guid? playerId = null, State? gameState = null, int pageNumber = 1, int pageSize = 100)
    {
        PlayerId = playerId;
        GameState = gameState;
        PageNumber = pageNumber;
        PageSize = pageSize;
        
        var validator = new GameFilterValidator();
        validator.ValidateAndThrow(this);
    }
}