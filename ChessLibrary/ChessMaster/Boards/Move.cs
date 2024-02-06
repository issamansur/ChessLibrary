using System.Text.RegularExpressions;
using ChessMaster.Figures;

namespace ChessMaster.Boards;

public struct Move
{
    // Fields and Properties
    private static Regex MovePattern => new Regex("^[kqrbnp][a-h][1-8][a-h][1-8]([kqrbnp])?$");
    
    public readonly Figure Figure;
    public readonly Field From;
    public readonly Field To;
    public readonly Figure? CapturedFigure;
    
    // Constructors
    public Move(Figure figure, Field from, Field to, Figure? capturedFigure = null)
    {
        Figure = figure;
        From = from;
        To = to;
        CapturedFigure = capturedFigure;
    }
    
    public static Move FromString(string move)
    {
        if (!MovePattern.IsMatch(move.ToLower()))
        {
            throw new ArgumentException("Invalid move: Invalid format");
        }
        
        var figure = Figure.FromChar(move[0]);
        var from = Field.FromString(move[..2]);
        var to = Field.FromString(move[2..4]);
        var capturedFigure = move.Length == 5 ? Figure.FromChar(move[4]) : null;
        
        return new Move(figure, from, to, capturedFigure);
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
        return $"{Figure}{From}{To}{CapturedFigure}"; // ?
    }
}