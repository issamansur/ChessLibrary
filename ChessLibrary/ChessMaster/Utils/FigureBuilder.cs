using ChessMaster.Figures;
using ChessMaster.States;

namespace ChessMaster.Utils;

public class FigureBuilder
{
    private char Symbol;
    private Color Color;
    
    public FigureBuilder WithSymbol(char symbol)
    {
        Symbol = symbol;
        return this;
    }
    /*
    public FigureBuilder WithColor(Color color)
    {
        Color = color;
        return this;
    }
    */
    
    public Figure? Build()
    {
        return Symbol switch
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
}