using ChessMaster.Boards;
using ChessMaster.States;

namespace ChessMaster.Figures;

public class King: Figure
{
    public override string Symbol { get; protected init; } = "K";

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
        
        // 2. King is not in check
        // 3. King does not pass through a square that is attacked by an enemy piece
        // 4. King does not end up in check
        
        // 5. There are no pieces between the king and the rook
        Field rock = Castling.GetRockPosition(move.To);
        return
            board.CanMoveFromTo(move.From, rock, move.Direction);
    }
    
    public override bool CanMove(Board board, Move move)
    {
        // Can move if:
        // 1. AbsDiffX and AbsDiffY are both <= 1
        // 2. Castling
        return CanSimpleMove(move) || CanCastle(board, move);
    }
}