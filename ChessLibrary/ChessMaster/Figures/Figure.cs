namespace ChessMaster.Figures;

public abstract class Figure
{
    public abstract string Symbol { get; protected init; }
    public Color Color { get; private init; }
    
    public Figure(Color color)
    {
        Color = color;
    }
    
    public abstract bool CanMove(Field fromTo, Field fieldTo);
    
    
    public override string ToString()
    {
        return Color == Color.White ? Symbol.ToUpper() : Symbol.ToLower();
    }
    
    public static Figure? FromChar(char symbol)
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
            _ => null,
        };
    }
}