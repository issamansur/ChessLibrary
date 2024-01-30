namespace ChessMaster.Figures;

public class Rook: Figure
{
    public override string Symbol { get; protected init; } = "R";

    public Rook(Color color) : base(color)
    {
    }

    public override bool CanMove(Field fromTo, Field fieldTo)
    {
        throw new NotImplementedException();
    }
}