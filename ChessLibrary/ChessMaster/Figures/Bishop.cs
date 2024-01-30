namespace ChessMaster.Figures;

public class Bishop: Figure
{
    public override string Symbol { get; protected init; } = "B";

    public Bishop(Color color) : base(color)
    {
    }

    public override bool CanMove(Field fromTo, Field fieldTo)
    {
        throw new NotImplementedException();
    }
}