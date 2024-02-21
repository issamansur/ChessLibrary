using ChessMaster.Domain.Boards;
using ChessMaster.Domain.States;


namespace ChessMaster.Domain.Figures;

public class Rook: Figure
{
    public bool IsJustMoved { get; private set; }
    
    public Rook(Color color, bool isJustMoved = false) : base(color)
    {
        IsJustMoved = isJustMoved;
    }
    
    public override void Move()
    {
        IsJustMoved = true;
    }
    
    public override bool CanMove(Board board, Move move)
    {
        return (move.AbsDiffX == 0 || move.AbsDiffY == 0) && board.CanMoveFromTo(move);
    }
}