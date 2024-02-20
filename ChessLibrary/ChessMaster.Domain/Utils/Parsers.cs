using System.Text;
using System.Text.RegularExpressions;
using ChessMaster.Domain.Boards;
using ChessMaster.Domain.Figures;
using ChessMaster.Domain.States;

namespace ChessMaster.Domain.Utils;

public class Parsers
{
    // Color
    public static Color StringToColor(string color)
    {
        return color switch
        {
            "w" => Color.White,
            "b" => Color.Black,
            _ => throw new ArgumentException("Invalid color"),
        };
    }
    
    public static string ColorToString(Color color)
    {
        return color switch
        {
            Color.White => "w",
            Color.Black => "b",
            _ => throw new ArgumentException("Invalid color"),
        };
    }
    
    // Figure
    public static Figure CharToFigure(char symbol)
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
    
    public static string FigureToString(Figure? figure)
    {
        return figure switch
        {
            King => figure.Color == Color.White ? "K" : "k",
            Queen => figure.Color == Color.White ? "Q" : "q",
            Rook => figure.Color == Color.White ? "R" : "r",
            Bishop => figure.Color == Color.White ? "B" : "b",
            Knight => figure.Color == Color.White ? "N" : "n",
            Pawn => figure.Color == Color.White ? "P" : "p",
            null => "",
            _ => throw new ArgumentException("Invalid figure"),
        };
    }
    
    
    public static char FigureToUnicode(Figure? figure)
    {
        return FigureToString(figure) switch
        {
            "K" => '♚',
            "Q" => '♛',
            "R" => '♜',
            "B" => '♝',
            "N" => '♞',
            "P" => '♟',
            "k" => '♔',
            "q" => '♕',
            "r" => '♖',
            "b" => '♗',
            "n" => '♘',
            "p" => '♙',
            _ => throw new ArgumentException("Invalid figure"),
        };
    }
    
    // Field
    private const string Alphabet = "abcdefgh";
    private static Regex FieldPattern => new Regex("^[a-h][1-8]$");
    
    public static Field StringToField(string field)
    {
        if (!FieldPattern.IsMatch(field.ToLower()))
        {
            throw new ArgumentException("Invalid field"); 
        }
        var x = Alphabet.IndexOf(field.ToLower()[0]);
        var y = int.Parse(field[1].ToString()) - 1;
        
        return new Field(x, y);
    }
    
    public static string FieldToString(Field field)
    {
        return $"{Alphabet[field.X]}{field.Y + 1}";
    }
    
    // Move
    private static Regex MovePattern => new Regex("^[kqrbnp][a-h][1-8][a-h][1-8]([kqrbnp])?$");

    public static Move StringToMove(string move)
    {
        if (!MovePattern.IsMatch(move.ToLower()))
        {
            throw new ArgumentException("Invalid move: Invalid format");
        }
        
        var figure = CharToFigure(move[0]);
        var from = StringToField(move[1..3]);
        var to = StringToField(move[3..5]);
        var promotedFigure = move.Length == 6 ? CharToFigure(move[5]) : null;
        
        return new Move(figure, from, to, promotedFigure);
    }
    
    public static string MoveToString(Move move)
    {
        return $"{FigureToString(move.Figure)}" +
               $"{FieldToString(move.From)}" +
               $"{FieldToString(move.To)}" +
               $"{FigureToString(move.PromotedFigure)}";
    }
    
    // Castling
    public static readonly Regex CastlingPattern = new Regex("^(K?Q?k?q?|-)$");
    
    public static Castling StringToCastling(string castlingFen)
    {
        if (!CastlingPattern.IsMatch(castlingFen))
        {
            throw new ArgumentException("Invalid castling");
        }
        
        bool castleE1C1 = castlingFen.Contains('K');
        bool castleE1G1 = castlingFen.Contains('Q');
        bool castleE8C8 = castlingFen.Contains('k');
        bool castleE8G8 = castlingFen.Contains('q');

        return new Castling(
            castleE1C1,
            castleE1G1,
            castleE8C8,
            castleE8G8
        );
    }
    
    public static string CastlingToString(Castling castling)
    {
        var castlingFen = new StringBuilder();
        
        if (castling.CanCastleE1C1)
        {
            castlingFen.Append('K');
        }
        if (castling.CanCastleE1G1)
        {
            castlingFen.Append('Q');
        }
        if (castling.CanCastleE8C8)
        {
            castlingFen.Append('k');
        }
        if (castling.CanCastleE8G8)
        {
            castlingFen.Append('q');
        }
        if (castlingFen.Length == 0)
        {
            castlingFen.Append('-');
        }

        return castlingFen.ToString();
    }
    
    // Board
    public static Figure?[,] StringToFigures(string fenFigures)
    {
        Figure?[,] figures = new Figure?[8, 8];
        
        var rows = fenFigures.Split('/');
        if (rows.Length != 8)
        {
            throw new ArgumentException("Invalid fen (Piece Placement): must contain 8 rows");
        }

        for (var y = 0; y < 8; y++)
        {
            var row = rows[y];
            var x = 0;
            foreach (var c in row)
            {
                if (char.IsDigit(c))
                {
                    x += int.Parse(c.ToString());
                }
                else
                {
                    figures[x, 7 - y] = CharToFigure(c);
                    x++;
                }
                if (x > 8)
                    throw new ArgumentException($"Invalid fen (Piece Placement): ...{row}...");
            }

            if (x != 8)
            {
                throw new ArgumentException($"Invalid fen (Piece Placement): ...{row}...");
            }
        }
        
        return figures;
    }
    
    public static string FiguresToString(Figure?[,] figures)
    {
        var fenFigures = new StringBuilder();
        
        for (var y = 0; y < 8; y++)
        {
            var empty = 0;
            for (var x = 0; x < 8; x++)
            {
                var figure = figures[x, 7 - y];
                if (figure == null)
                {
                    empty++;
                }
                else
                {
                    if (empty > 0)
                    {
                        fenFigures.Append(empty);
                        empty = 0;
                    }
                    fenFigures.Append(figure);
                }
            }
            if (empty > 0)
            {
                fenFigures.Append(empty);
            }
            if (y < 7)
            {
                fenFigures.Append('/');
            }
        }

        return fenFigures.ToString();
    }
}