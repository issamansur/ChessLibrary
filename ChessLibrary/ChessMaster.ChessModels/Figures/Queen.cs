namespace ChessMaster.ChessModels.Figures;

public class Queen: Figure
{

    public Queen(Color color) : base(color)
    {
    }
    
    public override bool CanMove(Board board, Move move)
    {
        return board.CanMoveFromTo(move);
    }
    
    public override Queen Clone()
    {
        return new Queen(Color);
    }
}