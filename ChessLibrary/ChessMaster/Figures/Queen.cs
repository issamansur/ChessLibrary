namespace ChessMaster.Figures;

public class Queen: Figure
{
    public override string Symbol { get; protected init; } = "Q";

    public Queen(Color color) : base(color)
    {
    }

    public override bool CanMove(Field fromTo, Field fieldTo)
    {
        throw new NotImplementedException();
    }
}