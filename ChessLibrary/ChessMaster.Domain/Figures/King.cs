using ChessMaster.Domain.Boards;
using ChessMaster.Domain.States;

namespace ChessMaster.Domain.Figures;

public class King : Figure
{

    public King(Color color) : base(color)
    {
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

        // Check on Castling:
        // 1. Castling is available (King and Rook are on their initial squares)
        if (!board.Castling.CanCastle(move))
        {
            return false;
        }
        
        Field rookPos = Castling.GetRockPosition(move.To);
        Figure? rook = board[rookPos];
        if (rook is not Rook || rook.Color != move.Figure.Color)
        {
            return false;
        }

        // 2. There are no pieces between the king and the rook
        if (!board.CanMoveFromTo(move.From, rookPos, move.Direction))
        {
            return false;
        }

        // 3. King is not in check
        if (board.IsCheck(board.ActiveColor))
        {
            return false;
        }

        // 4. King does not pass through a square that is attacked by an enemy piece
        Move newMove1 = new Move(move.Figure, move.From, move.From + move.Direction);
        Board newBoard1 = board.Move(newMove1);
        if (newBoard1.IsCheck(board.ActiveColor))
        {
            return false;
        }

        // 5. King does not end up in check
        Move newMove2 = new Move(
            move.Figure,
            move.From + move.Direction,
            move.To);
        newBoard1.ActiveColor = newBoard1.ActiveColor.ChangeColor();
        Board newBoard2 = newBoard1.Move(newMove2);
        return !newBoard2.IsCheck(board.ActiveColor);
    }

    public override bool CanMove(Board board, Move move)
    {
        // Can move if:
        // 1. AbsDiffX and AbsDiffY are both <= 1
        // 2. Castling
        return CanSimpleMove(move) || CanCastle(board, move);
    }
}