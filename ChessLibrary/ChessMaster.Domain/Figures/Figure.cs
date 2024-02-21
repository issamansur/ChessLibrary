using ChessMaster.Domain.Boards;
using ChessMaster.Domain.States;

namespace ChessMaster.Domain.Figures;

public abstract class Figure
{
    // Properties
    public Color Color { get; private init; }
    
    // Constructors
    public Figure(Color color)
    {
        Color = color;
    }
    
    // Methods
    public virtual void Move() { }
    public abstract bool CanMove(Board board, Move move);

    // Overrides

    // operators == and !=
    public static bool operator ==(Figure? a, Figure? b)
    {
        return a?.Color == b?.Color;
    }

    public static bool operator !=(Figure? a, Figure? b)
    {
        return !(a == b);
    }
}