namespace ChessMaster.Domain.Entities;

public class GameResult
{
    public Guid GameId { get; private set; }
    
    public Guid WhitePlayerId { get; private set; }
    public Guid BlackPlayerId { get; private set; }
    public Guid WinnerId { get; private set; }
    
    public DateTime CreationTime { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    
    public string LastPosition { get; private set; }
    
    public GameResult(
        Guid gameId,
        Guid whitePlayerId,
        Guid blackPlayerId,
        Guid winnerId,
        DateTime creationTime,
        DateTime startTime,
        DateTime endTime,
        string lastPosition
        )
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(gameId));
        }
        if (whitePlayerId == Guid.Empty)
        {
            throw new ArgumentException("WhitePlayerId cannot be empty", nameof(whitePlayerId));
        }
        if (blackPlayerId == Guid.Empty)
        {
            throw new ArgumentException("BlackPlayerId cannot be empty", nameof(blackPlayerId));
        }
        if (winnerId == Guid.Empty)
        {
            throw new ArgumentException("WinnerId cannot be empty", nameof(winnerId));
        }
        if (creationTime == DateTime.MinValue)
        {
            throw new ArgumentException("CreationTime cannot be empty", nameof(creationTime));
        }
        if (startTime == DateTime.MinValue)
        {
            throw new ArgumentException("StartTime cannot be empty", nameof(startTime));
        }
        if (endTime == DateTime.MinValue)
        {
            throw new ArgumentException("EndTime cannot be empty", nameof(endTime));
        }
        if (string.IsNullOrWhiteSpace(lastPosition))
        {
            throw new ArgumentException("LastPosition cannot be empty", nameof(lastPosition));
        }
        
        GameId = gameId;
        WhitePlayerId = whitePlayerId;
        BlackPlayerId = blackPlayerId;
        WinnerId = winnerId;
        CreationTime = creationTime;
        StartTime = startTime;
        EndTime = endTime;
        LastPosition = lastPosition;
    }

    public static GameResult Create(
        Game game,
        Guid winnerId,
        DateTime endTime
    )
    {
        var gameId = game.Id;
        var whitePlayerId = (Guid)game.WhitePlayerId!;
        var blackPlayerId = (Guid)game.BlackPlayerId!;
        var creationTime = game.CreationTime;
        var startTime = (DateTime)game.StartTime!;
        var lastPosition = game.FEN;
        
        return new GameResult(
            gameId,
            whitePlayerId,
            blackPlayerId,
            winnerId,
            creationTime,
            startTime,
            endTime,
            lastPosition
        );
    }
}