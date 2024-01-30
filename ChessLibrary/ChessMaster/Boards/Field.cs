using ChessMaster.Figures;

namespace ChessMaster;


public struct Field
{
    // Constants
    const string ALPHABET = "abcdefgh";
    
    // Static fields
    public static Field None => new(-1, -1);
    
    // Properties
    public int X { get; private init; }
    public int Y { get; private init; }
    
    public Figure? Figure { get; set; }
    
    // Constructor
    public Field(int x, int y)
    {
        X = x;
        Y = y;
        
        Figure = null;
    }
    
    // From string to Field
    public static Field FromString(string square)
    {
        if (square.Length != 2)
        {
            return None; // or throw exception
        }
        
        int x = ALPHABET.IndexOf(square.ToLower()[0]);
        int y = int.Parse(square[1].ToString()) - 1;
        
        return new Field(x, y);
    }
    
    // From Field to string
    public override string ToString()
    {
        return $"{ALPHABET[X]}{Y + 1}";
    }
}