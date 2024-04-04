using System.Text.RegularExpressions;
using ChessMaster.ChessModels.Figures;
using ChessMaster.ChessModels.Utils;

namespace ChessMaster.ChessModels.Boards;

public class Move
{
    // Fields and Properties
    public Figure Figure { get; internal set; }
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
    
    // Methods
    public int DiffX => To.X - From.X; // or (To-From).X ?
    public int DiffY => To.Y - From.Y;
    public int AbsDiffX => Math.Abs(DiffX);
    public int AbsDiffY => Math.Abs(DiffY);
    public Field Direction => new(Math.Sign(DiffX), Math.Sign(DiffY));
}