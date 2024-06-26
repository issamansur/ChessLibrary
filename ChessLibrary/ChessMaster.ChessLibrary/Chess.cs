using ChessMaster.ChessLibrary.Boards;
using ChessMaster.ChessLibrary.Figures;
using ChessMaster.ChessLibrary.States;
using ChessMaster.ChessLibrary.Utils;

namespace ChessMaster.ChessLibrary;

public class Chess
{
    // Fields and properties
    public Figure? this[int x, int y] => Board[x, y];
    internal Board Board { get; private set; }
    
    // 1.Active color of the current player
    // 2. Number of half moves since the last pawn advance or capture
    // 3. Number of the full move. It starts at 1, and is incremented after Black's move
    public Color ActiveColor { get; private set; }
    internal int HalfMoveClock { get; private set; }
    internal int FullMoveNumber { get; private set; }
    
    public GameState GameState { get; private set; }
    
    // Additional parts of chess game
    /*
    public List<Move> Moves { get; private set; }
    public List<string> History { get; private set; }
    */
    
    // Constructor
    public Chess()
    {
        Board = new Board();

        ActiveColor = Color.White;
        HalfMoveClock = 0;
        FullMoveNumber = 1;
        
        GameState = GameState.Playing;
    }
    
    public Chess(Board board, Color activeColor, int halfMoveClock, int fullMoveNumber)
    {
        Board = board;

        ActiveColor = activeColor;
        HalfMoveClock = halfMoveClock;
        FullMoveNumber = fullMoveNumber;
        
        UpdateState();
    }

    // Methods
    private void UpdateState()
    {
        // Check for checkmate, stalemate, check and update GameState
        if (Board.IsCheckmate(ActiveColor))
        {
            GameState = GameState.Checkmate;
        }
        else if (Board.IsStalemate(ActiveColor))
        {
            GameState = GameState.Stalemate;
        }
        else
        if (Board.IsCheck(ActiveColor))
        {
            GameState = GameState.Check;
        }
    }

    private bool CanMove(Move move)
    {
        if (move.From == move.To ||
            move.Figure.Color != ActiveColor ||
            Board[move.To]?.Color == ActiveColor
            )
        {
            return false;
        }

        return true;
    }
    
    public void Move(string stringMove)
    {
        if (GameState is GameState.Checkmate or GameState.Stalemate)
        {
            return;
        }
        
        Move move = StringParser.StringToMove(stringMove);
        
        // Check if the move is valid
        if (!CanMove(move))
        {
            throw new ArgumentException("Invalid move");
        }
        
        // Move
        Board = Board.Move(move);
        
        // Update properties:
        // 1. Update HalfMoveClock
        // 2. Update FullMoveNumber
        // 3. Update ActiveColor
        HalfMoveClock = move.Figure is Pawn ? 0 : HalfMoveClock + 1;
        if (ActiveColor == Color.White)
        {
            FullMoveNumber++;
        }
        ActiveColor = ActiveColor.ChangeColor();
        
        // Update GameState
        UpdateState();
    }
}