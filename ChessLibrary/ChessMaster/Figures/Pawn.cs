using ChessMaster.Boards;
using ChessMaster.States;

namespace ChessMaster.Figures;

public class Pawn: Figure
{
    public override string Symbol { get; protected init; } = "P";

    public Pawn(Color color) : base(color)
    {
    }

    public bool CanSimpleMove(Board board, Move move)
    {
        // Check on Direction
        if (move is not ({ DiffX: 0 }
            and ({ Figure.Color: Color.White, DiffY: 1 }
            or { Figure.Color: Color.Black, DiffY: -1 }
            )))
            return false;

        // Check on Capturing
        if (!CanCapturing(move))
        {
            return false;
        }

        // if field on board is empty - true, else - false
        return
            board[move.To] is null;
    }

    public bool CanDoubleMove(Board board, Move move)
    {
        // if starting/end place correct - false
        if (move is not ({ DiffX: 0 } 
            and ({ Figure.Color: Color.White, From.Y: 1, To.Y: 3 } 
            or { Figure.Color: Color.Black, From.Y: 6, To.Y: 4 }
            )))
           return false;

        // if fields on board is empty - true, else - false
        return
            board[move.From + move.Direction] is null &&
            board[move.To] is null;
    }

    public bool CanEat(Board board, Move move)
    {
        if (move is not { AbsDiffX: 1, AbsDiffY: 1 })
        {
            return false;
        }

        // Check on Capturing
        if (!CanCapturing(move))
        {
            return false;
        }

        return
            board[move.To] is not null;
    }

    public bool CanCapturing(Move move)
    {
        // if not last line
        if (move.To is not { Y: 0 or 7 })
        {
            return move.CapturedFigure is null;
        }

        // if last line
        if (move is { CapturedFigure: null or Pawn or King } ||
            move.CapturedFigure.Color != move.Figure.Color)
        {
            return false;
        }
        return true;
    }
    
    public override bool CanMove(Board board, Move move)
    {
        // Can move if:
        // 1. AbsDiffX is 0 and AbsDiffY is 1 (if on empty field)
        // 1. AND IF IT IS LAST LINE - CHECK IF CAPTURING CORRECT
        // 2. AbsDiffX is 0 and AbsDiffY is 2 (if on starting position and the field in front is empty)
        // 3. AbsDiffX is 1 and AbsDiffY is 1 (if on non-empty field)
        return 
            CanSimpleMove(board, move) || 
            CanDoubleMove(board, move) ||
            CanEat(board, move);
    }
}