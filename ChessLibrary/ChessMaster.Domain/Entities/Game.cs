using ChessMaster.ChessModels;
using ChessMaster.ChessModels.States;
using ChessMaster.ChessModels.Utils;

namespace ChessMaster.Domain.Entities;

public class Game
{
    // For created game
    public Guid Id { get; private set; }
    public Guid CreatorUserId { get; private set; }
    public DateTime CreationTime { get; private set; }
    
    public string FEN { get; private set; }
    public State State { get; private set; }
    
    // For games in progress
    public Guid? WhitePlayerId { get; private set; }
    public Guid? BlackPlayerId { get; private set; }
    
    // For finished games
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public Guid? WinnerId { get; private set; }
    
    public Game(
        Guid id,
        Guid creatorUserId,
        DateTime creationTime,
        string fen,
        State gameState,
        Guid? whitePlayerId,
        Guid? blackPlayerId,
        DateTime? startTime,
        DateTime? endTime,
        Guid? winnerId
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
        EndTime = endTime;
        WinnerId = winnerId;
    }
    
    public static Game Create(Guid creatorUserId)
    {
        var id = Guid.NewGuid();
        var creationTime = DateTime.UtcNow;
        var fen = ChessExt.DefaultFen;
        var gameState = State.Created;
        
        return new Game(
            id,
            creatorUserId,
            creationTime,
            fen,
            gameState,
            null,
            null,
            null,
            null,
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
        State = State.InProgress;
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
        
        if (chess.GameState == GameState.Checkmate)
        {
            Guid winnerId = (Guid)(chess.ActiveColor == Color.White ? BlackPlayerId! : WhitePlayerId!);
            Finish(winnerId);
        }
        else if (chess.GameState == GameState.Stalemate)
        {
            Finish(null);
        }
    }
    
    private void Finish(Guid? winnerId)
    {
        if (State != State.InProgress)
        {
            throw new InvalidOperationException("Game is not in progress");
        }
        
        State = State.Finished;
        EndTime = DateTime.UtcNow;
        WinnerId = winnerId;
    }
}