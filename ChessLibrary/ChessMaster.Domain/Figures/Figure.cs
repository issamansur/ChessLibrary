using ChessMaster.Domain.Boards;
using ChessMaster.Domain.States;

namespace ChessMaster.Domain.Figures;

public abstract class Figure
{
    // Properties
    public abstract string Symbol { get; protected init; }
    public Color Color { get; private init; }
    
    // Constructors
    public Figure(Color color)
    {
        Color = color;
    }
    
    public static Figure FromChar(char symbol)
    {
        return symbol switch
        {
            'K' => new King(Color.White),
            'k' => new King(Color.Black),
            'Q' => new Queen(Color.White),
            'q' => new Queen(Color.Black),
            'R' => new Rook(Color.White),
            'r' => new Rook(Color.Black),
            'B' => new Bishop(Color.White),
            'b' => new Bishop(Color.Black),
            'N' => new Knight(Color.White),
            'n' => new Knight(Color.Black),
            'P' => new Pawn(Color.White),
            'p' => new Pawn(Color.Black),
            _ => throw new ArgumentException("Invalid figure"),
        };
    }
    
    // Methods
    public abstract bool CanMove(Board board, Move move);
    
    public virtual char ToUnicode()
    {
        return Symbol[0] switch
        {
            'K' => '\u2654',
            'Q' => '\u2655',
            'R' => '\u2656',
            'B' => '\u2657',
            'N' => '\u2658',
            'P' => '\u2659',
            'k' => '\u265A',
            'q' => '\u265B',
            'r' => '\u265C',
            'b' => '\u265D',
            'n' => '\u265E',
            'p' => '\u265F',
            _ => '_'
        };
    }

    // Overrides

    // operators == and !=
    public static bool operator ==(Figure? a, Figure? b)
    {
        return a?.Symbol == b?.Symbol && a?.Color == b?.Color;
    }

    public static bool operator !=(Figure? a, Figure? b)
    {
        return !(a == b);
    }


    // ToString
    public override string ToString()
    {
        return Color == Color.White ? Symbol.ToUpper() : Symbol.ToLower();
    }
}