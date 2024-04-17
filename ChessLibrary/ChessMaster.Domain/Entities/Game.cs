using ChessMaster.ChessModels;
using ChessMaster.ChessModels.States;
using ChessMaster.ChessModels.Utils;

namespace ChessMaster.Domain.Entities;

public class Game
{
    public Guid Id { get; private set; }
    public Guid CreatorUserId { get; private set; }
    public DateTime CreationTime { get; private set; }
    public DateTime? StartTime { get; private set; }
    
    public Guid? WhitePlayerId { get; private set; }
    public Guid? BlackPlayerId { get; private set; }
    
    public string? FEN { get; private set; }
    public State State { get; private set; }
    
    public Game(
        Guid id,
        Guid creatorUserId,
        DateTime creationTime,
        Guid? whitePlayerId,
        Guid? blackPlayerId,
        string? fen,
        State gameState,
        DateTime? startTime
        )
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(id));
        }
        if (creatorUserId == Guid.Empty)
        {
            throw new ArgumentException("CreatorUserId cannot be empty", nameof(creatorUserId));
        }
        if (creationTime == DateTime.MinValue)
        {
            throw new ArgumentException("CreationTime cannot be empty", nameof(creationTime));
        }
        if (string.IsNullOrWhiteSpace(fen))
        {
            throw new ArgumentException("FEN cannot be empty", nameof(fen));
        }
        
        Id = id;
        CreatorUserId = creatorUserId;
        CreationTime = creationTime;
        WhitePlayerId = whitePlayerId;
        BlackPlayerId = blackPlayerId;
        FEN = fen;
        State = gameState;
        StartTime = startTime;
    }
    
    public static Game Create(Guid creatorUserId)
    {
        var id = Guid.NewGuid();
        var creationTime = DateTime.UtcNow;
        var gameState = State.Created;
        
        return new Game(
            id,
            creatorUserId,
            creationTime,
            null,
            null,
            null,
            gameState,
            null
        );
    }
    
    public void Join(Guid userId)
    {
        if (State != State.Created)
        {
            throw new InvalidOperationException("Game cannot be joined");
        }
        
        if (userId == CreatorUserId)
        {
            throw new InvalidOperationException("Creator cannot join the game");
        }
        
        Random random = new Random();
        if (random.Next(0, 2) == 0)
        {
            WhitePlayerId = CreatorUserId;
            BlackPlayerId = userId;
        }
        else
        {
            WhitePlayerId = userId;
            BlackPlayerId = CreatorUserId;
        }
        
        FEN = ChessExt.DefaultFen;
        State = State.Started;
        StartTime = DateTime.UtcNow;
    }
    
    public void UpdateFEN(string fen)
    {
        if (string.IsNullOrWhiteSpace(fen))
        {
            throw new ArgumentException("FEN cannot be empty", nameof(fen));
        }
        
        FEN = fen;
        Chess chess = Builders.ChessBuild(fen);
        
        if (chess.GameState is GameState.Checkmate or GameState.Stalemate)
        {
            Guid winnerId = (Guid)(chess.ActiveColor == Color.White ? BlackPlayerId! : WhitePlayerId!);
            Finish(winnerId);
        }
    }
    
    private GameResult Finish(Guid winnerId)
    {
        if (State != State.Started)
        {
            throw new InvalidOperationException("Game is not started");
        }
        
        if (winnerId != WhitePlayerId && winnerId != BlackPlayerId)
        {
            throw new ArgumentException("WinnerId is not a player in this game", nameof(winnerId));
        }

        State = State.Finished;

        GameResult gameResult = GameResult.Create(
            this,
            winnerId,
            DateTime.UtcNow
        );

        return gameResult;
    }
}