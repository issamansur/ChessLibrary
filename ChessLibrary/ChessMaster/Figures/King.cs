namespace ChessMaster.Figures;

public class King: Figure
{
    public override string Symbol { get; protected init; } = "K";

    public King(Color color) : base(color)
    {
    }

    public override bool CanMove(Field fromTo, Field fieldTo)
    {
        throw new NotImplementedException();
    }
}