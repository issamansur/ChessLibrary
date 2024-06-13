using ChessMaster.ChessLibrary.Boards;
using ChessMaster.ChessLibrary.States;

namespace ChessMaster.ChessLibrary.Figures;

public class Bishop: Figure
{

    public Bishop(Color color) : base(color)
    {
    }
    
    public override bool CanMove(Board board, Move move)
    {
        return move.AbsDiffX == move.AbsDiffY && board.CanMoveFromTo(move);
    }

    public override Bishop Clone()
    {
        return new Bishop(Color);
    }
}