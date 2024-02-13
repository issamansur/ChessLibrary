using ChessMaster.Boards;
using ChessMaster.States;

namespace ChessMaster.Figures;

public class Knight: Figure
{
    public override string Symbol { get; protected init; } = "N";

    public Knight(Color color) : base(color)
    {
    }
    
    public override bool CanMove(Board board, Move move)
    {
        return move is { AbsDiffX: 1, AbsDiffY: 2 } or { AbsDiffX: 2, AbsDiffY: 1 };
    }
}