using ChessMaster.Boards;
using ChessMaster.States;

namespace ChessMaster.Figures;

public class Pawn: Figure
{
    public override string Symbol { get; protected init; } = "P";

    public Pawn(Color color) : base(color)
    {
    }
    
    public override bool CanMove(Board board, Move move)
    {
        // Can move if:
        // 1. AbsDiffX is 0 and AbsDiffY is 1 (if on empty field)
        // 2. AbsDiffX is 0 and AbsDiffY is 2 (if on starting position and the field in front is empty)
        // 3. AbsDiffX is 1 and AbsDiffY is 1 (if on non-empty field)
        return move is { AbsDiffX: 0, AbsDiffY: 1 } && board[move.To] == null ||
               move is { AbsDiffX: 0, AbsDiffY: 2, From.Y: 1 or 6 } && board[move.Direction] == null && board[move.To] == null ||
               move is { AbsDiffX: 1, AbsDiffY: 1 } && board[move.To] != null;
    }
}