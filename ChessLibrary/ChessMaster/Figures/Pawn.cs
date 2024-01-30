namespace ChessMaster.Figures;

public class Pawn: Figure
{
    public override string Symbol { get; protected init; } = "P";

    public Pawn(Color color) : base(color)
    {
    }

    public override bool CanMove(Field fromTo, Field fieldTo)
    {
        throw new NotImplementedException();
    }
}