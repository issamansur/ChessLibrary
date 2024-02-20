using System.Text.RegularExpressions;
using ChessMaster.Boards;
using ChessMaster.Figures;

namespace ChessMaster.Utils;

public class MoveBuilder
{
    private static Regex MovePattern => new Regex("^[kqrbnp][a-h][1-8][a-h][1-8]([kqrbnp])?$");

    private Figure Figure { get; set; }
    private Field From { get; set; }
    private Field To { get; set; }
    private Figure? CapturedFigure { get; set; }

    public MoveBuilder FigureType(Figure figure)
    {
        Figure = figure;
        return this;
    }

    public MoveBuilder FromField(Field from)
    {
        From = from;
        return this;
    }

    public MoveBuilder ToField(Field to)
    {
        To = to;
        return this;
    }

    public MoveBuilder CapturedFigureType(Figure? capturedFigure)
    {
        CapturedFigure = capturedFigure;
        return this;
    }

    public MoveBuilder FromMoveString(string moveString)
    {
        if (!MovePattern.IsMatch(moveString.ToLower()))
        {
            throw new ArgumentException("Invalid move: Invalid format");
        }

        Figure = Figure.FromChar(moveString[0]);
        From = Field.FromString(moveString[1..3]);
        To = Field.FromString(moveString[4..6]);
        CapturedFigure = moveString.Length == 6 ? Figure.FromChar(moveString[8]) : null;

        return this;
    }

    public MoveBuilder FromMove(Move move)
    {
        Figure = move.Figure;
        From = move.From;
        To = move.To;
        CapturedFigure = move.CapturedFigure;

        return this;
    }

    public Move Build()
    {
        return new Move(Figure, From, To, CapturedFigure);
    }

    public string ToMoveString()
    {
        return $"{Figure}{From}{To}{CapturedFigure}";
    }
}