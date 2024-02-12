using ChessMaster.Boards;
using ChessMaster.States;


namespace ChessMaster.Figures;

public class Rook: Figure
{
    public override string Symbol { get; protected init; } = "R";

    public Rook(Color color) : base(color)
    {
    }
    
    public override bool CanMove(Board board, Move move)
    {
        return (move.AbsDiffX == 0 || move.AbsDiffY == 0) && board.CanMoveFromTo(move);
    }
}