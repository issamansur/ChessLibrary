using System.Security.Cryptography;

namespace ChessLibrary;

public class Moves
{
    private FigureMoving FigMov { get; set; }
    private Board Board { get; init; }

    public Moves(Board board)
    {
        Board = board;
    }

    public bool CanMove(FigureMoving fm)
    {
        FigMov = fm;
        return CanMoveFrom() && CanMoveTo() && CanFigureMove();
    }

    private bool CanMoveFrom()
    {
        return FigMov.From.OnBoard() &&
               Board.GetFigureAt(FigMov.From).GetColor() == Board.MoveColor &&
               FigMov.Figure.GetColor() == Board.MoveColor;
    }

    private bool CanMoveTo()
    {
        return FigMov.To.OnBoard() &&
               Board.GetFigureAt(FigMov.To).GetColor() != Board.MoveColor;
    }

    private bool CanKingMove()
    {
        return FigMov is { AbsDeltaX: <= 1, AbsDeltaY: <= 1 } || 
               (Board.IsCanCastleNow(FigMov) && CanHorizontalMove());
    }

    private bool CanStraightMove()
    {
        Square movement = FigMov.From;
        do
        {
            movement = new Square(movement.X + FigMov.SignDeltaX, movement.Y + FigMov.SignDeltaY);
            if (movement == FigMov.To)
            {
                return true;
            }
        } while (movement.OnBoard() && Board.GetFigureAt(movement) == Figure.None);

        return false;
    }

    private bool CanHorizontalMove()
    {
        return FigMov is { SignDeltaX: 0 } or { SignDeltaY: 0 } && CanStraightMove();
    }

    private bool CanVerticalMove()
    {
        return FigMov.SignDeltaX != 0 && FigMov.SignDeltaY != 0 && CanStraightMove();
    }

    private bool CanKnightMove()
    {
        return FigMov is { AbsDeltaX: 1, AbsDeltaY: 2 } or { AbsDeltaX: 2, AbsDeltaY: 1 };
    }
    
    private bool CanPawnGo(int stepY)
    {
        if (Board.GetFigureAt(FigMov.To) == Figure.None &&
            FigMov.DeltaX == 0 && FigMov.DeltaY == stepY)
            return true;
        return false;
    }

    private bool CanPawnJump(int stepY)
    {
        if (Board.GetFigureAt(FigMov.To) == Figure.None &&
            FigMov.DeltaX == 0 && FigMov.DeltaY == 2 * stepY &&
            FigMov.From.Y is 1 or 6 && 
            Board.GetFigureAt(new Square(FigMov.From.X, FigMov.From.Y + stepY)) == 0)
            return true;
        return false;
    }
    
    private bool CanPawnEat(int stepY)
    {
        return FigMov.DeltaX == 1 &&
               FigMov.DeltaY == stepY &&
               (Board.GetFigureAt(FigMov.To) != 0 || FigMov.To == Board.EnPassant);
    }
    
    private bool CanPawnMove()
    {
        if (FigMov.From.Y is < 1 or > 6)
            return false;
        int stepY = FigMov.Figure.GetColor() == Color.White ? 1 : -1;
        return CanPawnGo(stepY) || CanPawnJump(stepY) || CanPawnEat(stepY);
    }

    private bool CanFigureMove()
    {
        return
            FigMov.From != FigMov.To &&
            FigMov.Figure switch
            {
                Figure.None => false,
                Figure.WhiteKing => CanKingMove(),
                Figure.WhiteQueen => CanStraightMove(),
                Figure.WhiteRook => CanHorizontalMove(),
                Figure.WhiteBishop => CanVerticalMove(),
                Figure.WhiteKnight => CanKnightMove(),
                Figure.WhitePawn => CanPawnMove(),
                Figure.BlackKing => CanKingMove(),
                Figure.BlackQueen => CanStraightMove(),
                Figure.BlackRook => CanHorizontalMove(),
                Figure.BlackBishop => CanVerticalMove(),
                Figure.BlackKnight => CanKnightMove(),
                Figure.BlackPawn => CanPawnMove(),
                _ => false
            };
    }
}