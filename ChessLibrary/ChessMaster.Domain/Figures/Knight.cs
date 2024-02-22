using ChessMaster.Domain.Boards;
using ChessMaster.Domain.States;

namespace ChessMaster.Domain.Figures;

public class Knight: Figure
{

    public Knight(Color color) : base(color)
    {
    }
    
    public override bool CanMove(Board board, Move move)
    {
        return move is { AbsDiffX: 1, AbsDiffY: 2 } or { AbsDiffX: 2, AbsDiffY: 1 };
    }
    
    public override Knight Clone()
    {
        return new Knight(Color);
    }
}