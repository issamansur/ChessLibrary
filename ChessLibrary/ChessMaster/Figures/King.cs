using ChessMaster.Boards;
using ChessMaster.States;

namespace ChessMaster.Figures;

public class King: Figure
{
    public override string Symbol { get; protected init; } = "K";

    public King(Color color) : base(color)
    {
    }
    
    public override bool CanMove(Board board, Move move)
    {
        // Can move if:
        // 1. AbsDiffX and AbsDiffY are both <= 1
        // 2. Castling
        return move is { AbsDiffX: <= 1, AbsDiffY: <= 1 } || 
               move is { AbsDiffX: 2, DiffY: 0 } && board.CanCastleTo(move);
    }
}