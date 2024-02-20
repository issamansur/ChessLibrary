using System.Text.RegularExpressions;
using ChessMaster.Boards;
using ChessMaster.Figures;

namespace ChessMaster.States;

public class Castling
{
    // Fields and Properties
    public static readonly Regex CastlingPattern = new Regex("^(K?Q?k?q?|-)$");

    private bool CanCastleE1C1 { get; set; }
    private bool CanCastleE1G1 { get; set; }
    private bool CanCastleE8C8 { get; set; }
    private bool CanCastleE8G8 { get; set; }
    
    // Constructors
    public Castling(string castlingFen)
    {
        if (!CastlingPattern.IsMatch(castlingFen))
        {
            throw new ArgumentException("Invalid castling");
        }
        
        CanCastleE1C1 = castlingFen.Contains('K');
        CanCastleE1G1 = castlingFen.Contains('Q');
        CanCastleE8C8 = castlingFen.Contains('k');
        CanCastleE8G8 = castlingFen.Contains('q');
    }
    
    // Methods
    public bool CanCastle(Move move) //?
    {
        return (move.From.ToString(), move.To.ToString()) switch
        {
            ("e1", "g1") => CanCastleE1G1,
            ("e1", "c1") => CanCastleE1C1,
            ("e8", "g8") => CanCastleE8G8,
            ("e8", "c8") => CanCastleE8C8,
            _ => false
        };
    }

    public static Field GetRockPosition(Field to)
    {
        return to.ToString() switch
        {
            "g1" => Field.FromString("h1"),
            "c1" => Field.FromString("a1"),
            "g8" => Field.FromString("h8"),
            "c8" => Field.FromString("a8"),
            _ => throw new NotImplementedException()
        };
    } 

    public void Update(Move move)
    {
        if (move.Figure is King)
        {
            switch (move.Figure.Color)
            {
                case Color.White:
                    CanCastleE1C1 = false;
                    CanCastleE1G1 = false;
                    break;
                case Color.Black:
                    CanCastleE8C8 = false;
                    CanCastleE8G8 = false;
                    break;
            }
        }
        else if (move.Figure is Rook)
        {
            switch (move.From.ToString())
            {
                case "a1":
                    CanCastleE1C1 = false;
                    break;
                case "h1":
                    CanCastleE1G1 = false;
                    break;
                case "a8":
                    CanCastleE8C8 = false;
                    break;
                case "h8":
                    CanCastleE8G8 = false;
                    break;
            }
        }
    }

    // Overrides
    public override string ToString()
    {
        string castlingFen = "";
        
        if (CanCastleE1C1)
        {
            castlingFen += "K";
        }
        if (CanCastleE1G1)
        {
            castlingFen += "Q";
        }
        if (CanCastleE8C8)
        {
            castlingFen += "k";
        }
        if (CanCastleE8G8)
        {
            castlingFen += "q";
        }

        return castlingFen;
    }
}
