namespace ChessMaster.Figures;

public class Knight: Figure
{
    public override string Symbol { get; protected init; } = "N";

    public Knight(Color color) : base(color)
    {
    }

    public override bool CanMove(Field fromTo, Field fieldTo)
    {
        throw new NotImplementedException();
    }
}