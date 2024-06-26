using ChessMaster.ChessLibrary.Boards;
using ChessMaster.ChessLibrary.States;

namespace ChessMaster.ChessLibrary.Figures;

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
    public abstract Figure Clone();

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