using ChessMaster.ChessLibrary.Boards;
using ChessMaster.ChessLibrary.States;

namespace ChessMaster.ChessLibrary.Figures;

public class King : Figure
{
    public bool IsJustMoved { get; private set; }

    public King(Color color, bool isJustMoved = false) : base(color)
    {
        IsJustMoved = isJustMoved;
    }
    
    public override void Move()
    {
        IsJustMoved = true;
    }

    private bool CanSimpleMove(Move move)
    {
        return move is { AbsDiffX: <= 1, AbsDiffY: <= 1 };
    }

    private bool CanCastle(Board board, Move move)
    {
        // Check on Direction
        if (move is not { AbsDiffX: 2, DiffY: 0 })
        {
            return false;
        }
        
        if (move.Figure.Color == Color.White && move.From is not {X: 4, Y: 0} ||
            move.Figure.Color == Color.Black && move.From is not {X: 4, Y: 7})
        {
            return false;
        }

        // Check on Castling:
        // 1. Get Rook Position and check if exist
        int rookX = move.DiffX > 1 ? 7 : 0;
        int rookY = move.From.Y;
        Field rookPos = new Field(rookX, rookY);
        if (board[rookPos] is not Rook rook || rook.Color != move.Figure.Color)
        {
            return false;
        }
        
        // 2. Castling is available (King and Rook are on their initial squares)
        if (IsJustMoved || rook.IsJustMoved)
        {
            return false;
        }

        // 3. There are no pieces between the king and the rook
        if (!board.CanMoveFromTo(move.From, rookPos, move.Direction))
        {
            return false;
        }

        // 3. King is not in check
        if (board.IsCheck(move.Figure.Color))
        {
            return false;
        }

        // 4. King does not pass through a square that is attacked by an enemy piece
        if (board.IsUnderAttack(move.From + move.Direction, move.Figure.Color.ChangeColor()))
        {
            return false;
        }
        
        return true;
    }

    public override bool CanMove(Board board, Move move)
    {
        return CanSimpleMove(move) || CanCastle(board, move);
    }

    public override King Clone()
    {
        return new King(Color, IsJustMoved);
    }
}