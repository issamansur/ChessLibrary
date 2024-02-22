using System.Text.RegularExpressions;
using ChessMaster.Domain.Boards;
using ChessMaster.Domain.Figures;
using ChessMaster.Domain.Utils;

namespace ChessMaster.Domain.States;

public class Castling
{
    // Fields and Properties
    internal bool CanCastleE1C1 { get; private set; }
    internal  bool CanCastleE1G1 { get; private set; }
    internal  bool CanCastleE8C8 { get; private set; }
    internal  bool CanCastleE8G8 { get; private set; }
    
    // Constructors
    public Castling()
    {
        CanCastleE1C1 = true;
        CanCastleE1G1 = true;
        CanCastleE8C8 = true;
        CanCastleE8G8 = true;
    }
    
    public Castling(bool canCastleE1C1, bool canCastleE1G1, bool canCastleE8C8, bool canCastleE8G8)
    {
        CanCastleE1C1 = canCastleE1C1;
        CanCastleE1G1 = canCastleE1G1;
        CanCastleE8C8 = canCastleE8C8;
        CanCastleE8G8 = canCastleE8G8;
    }
    
    // Methods
    public bool CanCastle(Move move) //?
    {
        return (
                StringParser.FieldToString(move.From), 
                StringParser.FieldToString(move.To)
        ) switch
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
        return StringParser.FieldToString(to) switch
        {
            "g1" => StringParser.StringToField("h1"),
            "c1" => StringParser.StringToField("a1"),
            "g8" => StringParser.StringToField("h8"),
            "c8" => StringParser.StringToField("a8"),
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
            switch (StringParser.FieldToString(move.From))
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
}
