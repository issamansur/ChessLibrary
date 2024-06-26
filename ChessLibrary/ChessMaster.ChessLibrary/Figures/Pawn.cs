using ChessMaster.ChessLibrary.Boards;
using ChessMaster.ChessLibrary.States;

namespace ChessMaster.ChessLibrary.Figures;

public class Pawn : Figure
{

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
        if (!CanPromote(move))
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

    public bool CanCapture(Board board, Move move)
    {
        // Check on Direction
        if (move is not ({ AbsDiffX: 1, }
            and ({ Figure.Color: Color.White, DiffY: 1 } 
            or { Figure.Color: Color.Black, DiffY: -1 }
            )))
        {
            return false;
        }

        // Check on Capturing
        if (!CanPromote(move))
        {
            return false;
        }

        return
            board[move.To] is not null || board.EnPassantTargetSquare == move.To;
    }

    public bool CanPromote(Move move)
    {
        // if not last line
        if (move.To is not { Y: 0 or 7 })
        {
            return move.PromotedFigure is null;
        }

        // if last line
        if (move is { PromotedFigure: null or Pawn or King } ||
            move.PromotedFigure.Color != move.Figure.Color)
        {
            return false;
        }

        return true;
    }

    public override bool CanMove(Board board, Move move)
    {
        return CanSimpleMove(board, move) || CanDoubleMove(board, move) || CanCapture(board, move);
    }

    public override Pawn Clone()
    {
        return new Pawn(Color);
    }
}