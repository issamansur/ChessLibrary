using System.Collections.ObjectModel;
using ChessMaster.Figures;

namespace ChessMaster;

public class Board
{
    private readonly Field[,] _fields;
    public Figure? this[int x, int y] => _fields[x, y].Figure;
    public Figure? this[Field field] => _fields[field.X, field.Y].Figure;
    
    // Constructor
    public Board(string boardFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR")
    {
        _fields = new Field[8, 8];
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++) 
            {
                _fields[x, y] = new Field(x, y);
            }
        }
        
        SetFigures(boardFen);
    }
    
    // Set figures
    private void SetFigures(string boardFen)
    {
        var rows = boardFen.Split('/');
        if (rows.Length != 8)
        {
            throw new ArgumentException("Invalid fen (Piece Placement): must contain 8 rows");
        }
        
        for (int y = 0; y < 8; y++)
        {
            var row = rows[y];
            int x = 0;
            foreach (var c in row)
            {
                if (char.IsDigit(c))
                {
                    x += int.Parse(c.ToString());
                }
                else
                {
                    _fields[x, y].Figure = Figure.FromChar(c);
                    x++;
                }
            }
            if (x!= 8)
            {
                throw new ArgumentException($"Invalid fen (Piece Placement): ...{row}...");
            }
        }
    }
}