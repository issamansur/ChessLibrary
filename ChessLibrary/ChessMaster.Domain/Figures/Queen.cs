using ChessMaster.Domain.Boards;
using ChessMaster.Domain.States;

namespace ChessMaster.Domain.Figures;

public class Queen: Figure
{

    public Queen(Color color) : base(color)
    {
    }
    
    public override bool CanMove(Board board, Move move)
    {
        return board.CanMoveFromTo(move);
    }
}