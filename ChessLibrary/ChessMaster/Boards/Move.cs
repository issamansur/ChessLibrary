using System.Text.RegularExpressions;
using ChessMaster.Figures;

namespace ChessMaster.Boards;

public class Move
{
    // Fields and Properties
    private static Regex MovePattern => new Regex("^[kqrbnp][a-h][1-8][a-h][1-8]([kqrbnp])?$");
    
    public readonly Figure Figure;
    public readonly Field From;
    public readonly Field To;
    public readonly Figure? PromotedFigure;
    
    // Constructors
    public Move(Figure figure, Field from, Field to, Figure? promotedFigure = null)
    {
        Figure = figure;
        From = from;
        To = to;
        PromotedFigure = promotedFigure;
    }
    
    public static Move FromString(string move)
    {
        if (!MovePattern.IsMatch(move.ToLower()))
        {
            throw new ArgumentException("Invalid move: Invalid format");
        }
        
        var figure = Figure.FromChar(move[0]);
        var from = Field.FromString(move[1..3]);
        var to = Field.FromString(move[3..5]);
        var promotedFigure = move.Length == 6 ? Figure.FromChar(move[5]) : null;
        
        return new Move(figure, from, to, promotedFigure);
    }
    
    // Methods
    public int DiffX => To.X - From.X; // or (To-From).X ?
    public int DiffY => To.Y - From.Y;
    public int AbsDiffX => Math.Abs(DiffX);
    public int AbsDiffY => Math.Abs(DiffY);
    public Field Direction => new(Math.Sign(DiffX), Math.Sign(DiffY));
    
    
    // Overrides
    public override string ToString()
    {
        return $"{Figure}{From}{To}{PromotedFigure}"; // ?
    }
}