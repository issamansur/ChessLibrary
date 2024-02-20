using ChessMaster.Boards;
using ChessMaster.States;

namespace ChessMaster.Figures;

public class Queen: Figure
{
    public override string Symbol { get; protected init; } = "Q";

    public Queen(Color color) : base(color)
    {
    }
    
    public override bool CanMove(Board board, Move move)
    {
        return board.CanMoveFromTo(move);
    }
}