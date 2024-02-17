using ChessMaster.Domain.Boards;
using ChessMaster.Domain.States;

namespace ChessMaster.Domain.Figures;

public class Bishop: Figure
{
    public override string Symbol { get; protected init; } = "B";

    public Bishop(Color color) : base(color)
    {
    }
    
    public override bool CanMove(Board board, Move move)
    {
        return move.AbsDiffX == move.AbsDiffY && board.CanMoveFromTo(move);
    }
}