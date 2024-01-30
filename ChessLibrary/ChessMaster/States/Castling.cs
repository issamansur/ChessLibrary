namespace ChessMaster;

public class Castling
{
    public bool CanCastleE1C1 { get; set; }
    public bool CanCastleE1G1 { get; set; }
    public bool CanCastleE8C8 { get; set; }
    public bool CanCastleE8G8 { get; set; }
    
    public Castling(string castlingFen)
    {
        if (castlingFen.Length is 0 or > 4)
        {
            throw new ArgumentException("Invalid fen (Castling): Length must be 1-4");
        }
        
        CanCastleE1C1 = castlingFen.Contains("K");
        CanCastleE1G1 = castlingFen.Contains("Q");
        CanCastleE8C8 = castlingFen.Contains("k");
        CanCastleE8G8 = castlingFen.Contains("q");
    }
    
    public bool CanCastle() //?
    {
        return CanCastleE1C1 || CanCastleE1G1 || CanCastleE8C8 || CanCastleE8G8;
    }

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